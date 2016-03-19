using System;
using System.Linq;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microsoft.Extensions.Logging;

namespace Microbrewit.Api.Calculations
{

    public class Calculation : ICalculation
    {
        private readonly ILogger<Calculation> _logger;
        private IFermentableElasticsearch _fermentableElasticsearch;
        private IFermentableRepository _fermentableRepository;
        
        public Calculation(IFermentableElasticsearch fermentableElasticsearch,IFermentableRepository fermentableRepository, ILogger<Calculation> logger)
        {
            _fermentableElasticsearch = fermentableElasticsearch;
            _fermentableRepository = fermentableRepository;
            _logger = logger;
        }

        public SRM CalculateSRM(Recipe recipe)
        {
            var srm = new SRM{SrmId = recipe.RecipeId};
            foreach (var mashStep in recipe.MashSteps)
            {
                var volume = recipe.Volume;
                if (mashStep.Volume > 0)
                    volume = mashStep.Volume;
                foreach (var fermentable in mashStep.Fermentables)
                {
                    srm.Standard += Math.Round(Formulas.MaltColourUnits(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Morey += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Mosher += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Daniels += Math.Round(Formulas.Daniels(fermentable.Amount, fermentable.Lovibond, volume), 0);
                }
            }

            return srm;
        }

        public SRMDto CalculateSRMDto(RecipeDto recipe)
        {
            var srm = new SRMDto();
            foreach (var item in recipe.Steps.Where(m => m.Type == "mash"))
            {
                var mashStepDto = (MashStepDto)item;
                var volume = recipe.Volume;
                if (mashStepDto.Volume > 0)
                    volume = mashStepDto.Volume;
                foreach (var fermentable in mashStepDto.Ingredients.Where(f => f != null && f.Type == "fermentable"))
                {
                    var temp = (FermentableStepDto) fermentable;
                    srm.Standard += Math.Round(Formulas.MaltColourUnits(temp.Amount, temp.Lovibond, volume), 0);
                    srm.Morey += Math.Round(Formulas.Morey(temp.Amount, temp.Lovibond, volume), 0);
                    srm.Mosher += Math.Round(Formulas.Morey(temp.Amount, temp.Lovibond, volume), 0);
                    srm.Daniels += Math.Round(Formulas.Daniels(temp.Amount, temp.Lovibond, volume), 0);
                }
            }

            return srm;
        }

        public IBU CalculateIBU(Recipe recipe)
        {
            var og = recipe.OG;
            var ibu = new IBU {IbuId = recipe.RecipeId};
           
            var tinseth = 0.0;
            var rager = 0.0;
            foreach (var boilStep in recipe.BoilSteps)
            {
                var tinsethUtilisation = Formulas.TinsethUtilisation(og, boilStep.Length);
                var ragerUtilisation = Formulas.RangerUtilisation(boilStep.Length);
                foreach (var hop in boilStep.Hops)
                {
                    var tinasethMgl = Formulas.TinsethMgl(hop.Amount, hop.AAValue, recipe.Volume);
                    tinseth += Formulas.TinsethIbu(tinasethMgl, tinsethUtilisation);
                    rager += Formulas.RangerIbu(hop.Amount, ragerUtilisation, hop.AAValue, recipe.Volume, og);
                }
            }
            
            ibu.Tinseth = Math.Round(tinseth, 1);
            ibu.Standard = Math.Round(tinseth, 1);
            ibu.Rager = Math.Round(rager, 1);
            return ibu;
        }

        public IBUDto CalculateIBUDto(RecipeDto recipe)
        {
            var og = recipe.OG;
            var ibu = new IBUDto();

            var tinseth = 0.0;
            var rager = 0.0;
            foreach (var item in recipe.Steps.Where(s => s.Type == "boil"))
            {
                var boilStep = (BoilStepDto)item;
                var tinsethUtilisation = Formulas.TinsethUtilisation(og, boilStep.Length);
                var ragerUtilisation = Formulas.RangerUtilisation(boilStep.Length);
                foreach (var temp in boilStep.Ingredients.Where(i => i.Type == "hop"))
                {
                    var hop = (HopStepDto) temp;
                    var tinasethMgl = Formulas.TinsethMgl(hop.Amount, hop.AAValue, recipe.Volume);
                    tinseth += Formulas.TinsethIbu(tinasethMgl, tinsethUtilisation);
                    rager += Formulas.RangerIbu(hop.Amount, ragerUtilisation, hop.AAValue, recipe.Volume, og);
                }
            }

            ibu.Tinseth = Math.Round(tinseth, 1);
            ibu.Standard = Math.Round(tinseth, 1);
            ibu.Rager = Math.Round(rager, 1);
            return ibu;
        }

        public double CalculateOG(Recipe recipe)
        {
            var og = 0.0;

            foreach (var fermentable in recipe.MashSteps.SelectMany(mashStep => mashStep.Fermentables))
            {
                var efficency = recipe.Efficiency;
                //if (fermentable.PPG <= 0)
                //{
                    var esFermentable = _fermentableElasticsearch.GetSingleAsync(fermentable.FermentableId).Result;
                    if (esFermentable != null && esFermentable.PPG > 0)
                    {
                        fermentable.PPG = esFermentable.PPG;
                        if (esFermentable.Type.Contains("Extract") || esFermentable.Type.Contains("Sugar"))
                            efficency = 100;
                        //og += Formulas.MaltOG(fermentable.Amount, esFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }
                    else
                    {
                        var efFermentable = _fermentableRepository.GetSingleAsync(fermentable.FermentableId).Result;
                        if (efFermentable != null && efFermentable.PPG != null)
                        {
                            fermentable.PPG = (int)efFermentable.PPG;
                            if (efFermentable.Type.Contains("Extract") || efFermentable.Type.Contains("Sugar"))
                                efficency = 100;
                        }
                        //og += Formulas.MaltOG(fermentable.Amount, (int)efFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }

                //}
                og += Formulas.MaltOG(fermentable.Amount, (int)fermentable.PPG, efficency, recipe.Volume);
            }
            return Math.Round(1 + og / 1000, 4);
        }

        public double CalculateOGDto(RecipeDto recipe)
        {
            var og = 0.0;

            try
            {
                var fermentables =
                    recipe.Steps.Where(s => s.Type == "mash")
                        .SelectMany(mashStep => ((MashStepDto) mashStep).Ingredients.Where(f => f != null && f.Type == "fermentable"));
                foreach (var fermentable in fermentables)
                {
                    var temp = (FermentableStepDto) fermentable;
                    var efficency = recipe.Efficiency;
                    //if (fermentable.PPG <= 0)
                    //{
                    var esFermentable = _fermentableElasticsearch.GetSingle(temp.FermentableId);
                    if (esFermentable != null && esFermentable.PPG > 0)
                    {
                        temp.PPG = esFermentable.PPG;
                        if (esFermentable.Type.Contains("Extract") || esFermentable.Type.Contains("Sugar"))
                            efficency = 100;
                        //og += Formulas.MaltOG(fermentable.Amount, esFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }
                    else
                    {
                        var efFermentable = _fermentableRepository.GetSingleAsync(temp.FermentableId).Result;
                        if (efFermentable.PPG != null)
                        {
                            temp.PPG = (int) efFermentable.PPG;
                            if (efFermentable.Type.Contains("Extract") || efFermentable.Type.Contains("Sugar"))
                                efficency = 100;
                        }
                        //og += Formulas.MaltOG(fermentable.Amount, (int)efFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }

                    //}
                    og += Formulas.MaltOG(temp.Amount, (int)temp.PPG, efficency, recipe.Volume);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                throw;
            }

            return Math.Round(1 + og / 1000, 4);
        }


        public ABV CalculateABV(Recipe recipe)
        {
            var abv = new ABV
            {
                AbvId = recipe.RecipeId,
                Miller = Math.Round(Formulas.MillerABV(recipe.OG, recipe.FG), 2),
                Simple = Math.Round(Formulas.SimpleABV(recipe.OG, recipe.FG), 2),
                Advanced = Math.Round(Formulas.AdvancedABV(recipe.OG, recipe.FG), 2),
                AdvancedAlternative = Math.Round(Formulas.AdvancedAlternativeABV(recipe.OG, recipe.FG), 2),
                AlternativeSimple = Math.Round(Formulas.SimpleAlternativeABV(recipe.OG, recipe.FG), 2),
                Standard = Math.Round(Formulas.MicrobrewitABV(recipe.OG, recipe.FG), 2)
            };

            return abv;
        }

        public ABVDto CalculateABVDto(RecipeDto recipe)
        {
            var abv = new ABVDto
            {
                Miller = Math.Round(Formulas.MillerABV(recipe.OG, recipe.FG), 2),
                Simple = Math.Round(Formulas.SimpleABV(recipe.OG, recipe.FG), 2),
                Advanced = Math.Round(Formulas.AdvancedABV(recipe.OG, recipe.FG), 2),
                AdvancedAlternative = Math.Round(Formulas.AdvancedAlternativeABV(recipe.OG, recipe.FG), 2),
                AlternativeSimple = Math.Round(Formulas.SimpleAlternativeABV(recipe.OG, recipe.FG), 2),
                Standard = Math.Round(Formulas.MicrobrewitABV(recipe.OG, recipe.FG), 2)
            };

            return abv;
        }
    }
}