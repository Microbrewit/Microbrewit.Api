using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.OptionsModel;

namespace Microbrewit.Api.Mapper
{
    public  class BeerXmlResolver : IBeerXmlResolver
    {
        private  readonly IOptions<DatabaseSettings> _databaseSettings;
        private  readonly IOptions<ElasticSearchSettings> _elasticSearchSettings;
        private  readonly IFermentableElasticsearch _fermentableElasticsearch;
        private  readonly IHopElasticsearch _hopElasticsearch;
        private  readonly IHopRepository _hopRepository;
        private  readonly IOtherElasticsearch _otherElasticsearch;
        private  readonly IYeastElasticsearch _yeastElasticsearch;
        private  readonly IOtherRepository _otherRepository;
        private  readonly IOtherService _otherService;
        public BeerXmlResolver(IOptions<DatabaseSettings> databaseSettings,IOptions<ElasticSearchSettings> elasticSearchSettings,
        IFermentableElasticsearch fermentableElasticsearch, IHopElasticsearch hopElasticsearch, IHopRepository hopRepository,
        IOtherElasticsearch otherElasticsearch, IYeastElasticsearch yeastElasticsearch, IOtherRepository otherRepository, IOtherService otherService)
        {
            _databaseSettings = databaseSettings;
            _elasticSearchSettings = elasticSearchSettings;
            _fermentableElasticsearch = fermentableElasticsearch;
            _hopElasticsearch = hopElasticsearch;
            _hopRepository = hopRepository;
            _otherElasticsearch = otherElasticsearch;
            _yeastElasticsearch = yeastElasticsearch;
            _otherRepository = otherRepository;
            _otherService = otherService;
        }

        
        public RecipeDto ResolveCore(Model.BeerXml.Recipe source)
        {
            try
            {
            var boilSize = (int)double.Parse(source.BoilSize, CultureInfo.InvariantCulture);
            var batchSize = (int)double.Parse(source.BatchSize, CultureInfo.InvariantCulture);
            double fg = 0;
            if (source.Fg != null)
                fg = double.Parse(source.Fg, CultureInfo.InvariantCulture);

            var recipeDto = new RecipeDto
            {
                //MashSteps = new List<MashStepDto>(),
                //BoilSteps = new List<BoilStepDto>(),
                //FermentationSteps = new List<FermentationStepDto>(),
                //SpargeStep = null, //new SpargeStepDto(),
                Steps = new List<IStepDto>(),
                Notes = source.Taste_Notes,
                //Sets 60min as standard.
                TotalBoilTime = (source.Boil_Time != null) ? double.Parse(source.Boil_Time, CultureInfo.InvariantCulture) : 60,
                Volume = (batchSize <= 0) ? boilSize : batchSize,
            };

            // <PRIMARY_AGE>
            if (source.PrimaryAge != null)
            {
                var primaryAge = (int)double.Parse(source.PrimaryAge, CultureInfo.InvariantCulture);
                var primaryTemp = (source.Primary_Temp == null) ? 0 : (int)double.Parse(source.Primary_Temp, CultureInfo.InvariantCulture);
                var fermentationStep = GetFermentationStepDto(recipeDto, primaryAge);
                if (fermentationStep != null)
                {
                    fermentationStep.Temperature = primaryTemp;
                    recipeDto.Steps.Add(fermentationStep);
                }
            }

            //<SECONDAY_AGE>
            if (source.Secondary_Age != null)
            {
                var secondaryAge = (int)double.Parse(source.Secondary_Age, CultureInfo.InvariantCulture);
                var primaryTemp = (source.Secondary_Temp == null) ? 0 : (int)double.Parse(source.Secondary_Temp, CultureInfo.InvariantCulture);
                var fermentationStep = GetFermentationStepDto(recipeDto, secondaryAge);
                if (fermentationStep != null)
                {
                    fermentationStep.Temperature = primaryTemp;
                    recipeDto.Steps.Add(fermentationStep);
                }
            }
            // <TERTIARYAGE>
            if (source.TertiaryAge != null)
            {
                var tertiaryAge = (int)double.Parse(source.TertiaryAge, CultureInfo.InvariantCulture);
                var tertiaryTemp = (source.TertiaryTemp == null) ? 0 : (int)double.Parse(source.TertiaryTemp, CultureInfo.InvariantCulture);
                var fermentationStep = GetFermentationStepDto(recipeDto, tertiaryAge);
                if (fermentationStep != null)
                {
                    fermentationStep.Temperature = tertiaryTemp;
                    recipeDto.Steps.Add(fermentationStep);
                }
            }

            if (!recipeDto.Steps.OfType<FermentationStepDto>().Any())
            {
                recipeDto.Steps.Add(new FermentationStepDto
                {
                    Length = 14,
                    Ingredients = new List<IIngredientStepDto>(),
                });

            }



            //Mash step from <MASH> <MASH_STEPS>
            if (source.Mash != null && source.Mash.MashSteps.Any())
            {
                foreach (var mashStep in source.Mash.MashSteps)
                {
                    var mashStepDto = GetMashStepDto(mashStep);
                    recipeDto.Steps.Add(mashStepDto);
                }
            }
            else
            {
                var mashStepDto = new MashStepDto
                {
                    Length = 60,
                    Temperature = 0,
                    Notes = "",
                    Ingredients =  new List<IIngredientStepDto>()
                };
                recipeDto.Steps.Add(mashStepDto);

            }
            //Fermentable
            if (source.Fermentables != null)
            {
                var totalBoilTime = (int) recipeDto.TotalBoilTime;
                var mashStep = GetMashStepDto(recipeDto, totalBoilTime) ??
                                recipeDto.Steps.OfType<MashStepDto>().FirstOrDefault();
                foreach (var fermentable in source.Fermentables)
                {
                    if (mashStep != null && (string.IsNullOrEmpty(fermentable.AddAfterBoil) || fermentable.AddAfterBoil.ToLower() == "false"))
                    {
                        IIngredientStepDto fermentableStepDto = GetFermentableStepDto(fermentable, mashStep);
                        mashStep.Ingredients.Add(fermentableStepDto);
                        break;
                    }
                    var fermentationSteps = recipeDto.Steps.OfType<FermentationStepDto>();
                    var fermentationStep = fermentationSteps?.FirstOrDefault();
                    if (fermentationStep != null && fermentable.AddAfterBoil.ToLower() == "true")
                    {
                        var fermentableStepDto = GetFermentableStepDto(fermentable, fermentationStep);
                        fermentationStep.Ingredients.Add(fermentableStepDto);
                    }
                }
            }
            //Hops
            if (source.Hops != null)
            {
                foreach (var hop in source.Hops)
                {
                    var time = (int)double.Parse(hop.Time, CultureInfo.InvariantCulture);
                    var hopStepDto = GetHopStepDto(hop);
                    if (string.Equals(hop.Use, "Boil", StringComparison.OrdinalIgnoreCase) || hop.Use == "")
                    {
                        var boilStep = GetBoilStepDto(recipeDto, time);
                        if (hopStepDto != null)
                            boilStep.Ingredients.Add(hopStepDto);
                    }

                    if (string.Equals(hop.Use, "First Wort"))
                    {
                        //TODO: add support for first wort.
                    }

                    if (string.Equals(hop.Use, "Mash") && string.Equals(hop.Use, "Aroma"))
                    {
                        var mashStep = GetMashStepDto(recipeDto, time) ?? recipeDto.Steps.OfType<MashStepDto>().FirstOrDefault();
                        if (hopStepDto != null)
                            mashStep.Ingredients.Add(hopStepDto);
                    }

                    if (hop.Use == "Dry Hop")
                    {
                        var fermentationSteps = recipeDto.Steps.OfType<FermentationStepDto>();
                        var fermentationStep = GetFermentationStepDto(recipeDto, time) ?? fermentationSteps.FirstOrDefault();
                        if (hopStepDto != null) 
                            fermentationStep?.Ingredients.Add(hopStepDto);
                    }
                }
            }

            if (source.Miscs != null)
            {
                foreach (var misc in source.Miscs)
                {
                    int time = 0;
                    if (misc.Time.Any())
                        time = (int)double.Parse(misc.Time, CultureInfo.InvariantCulture);
                    var othersStepDto = GetOthersStepDto(misc);
                    if (string.Equals(misc.Use, "Boil", StringComparison.OrdinalIgnoreCase))
                    {
                        var boilStep = GetBoilStepDto(recipeDto, time);
                        if (othersStepDto != null)
                            boilStep.Ingredients.Add(othersStepDto);
                    }
                    if (string.Equals(misc.Use, "Mash", StringComparison.OrdinalIgnoreCase))
                    {
                        var mashStep = GetMashStepDto(recipeDto, time) ?? recipeDto.Steps.OfType<MashStepDto>().FirstOrDefault();
                        if (othersStepDto != null)
                            mashStep.Ingredients.Add(othersStepDto);
                    }
                    if (string.Equals(misc.Use, "Primary", StringComparison.OrdinalIgnoreCase) || string.Equals(misc.Use, "Secondary", StringComparison.OrdinalIgnoreCase))
                    {
                        var fermentationSteps = recipeDto.Steps.OfType<FermentationStepDto>();
                        var fermentationStepDto = GetFermentationStepDto(recipeDto, time) ?? fermentationSteps.FirstOrDefault();
                        if (othersStepDto != null)
                            fermentationStepDto.Ingredients.Add(othersStepDto);
                    }
                    if (string.Equals(misc.Use, "Bottling", StringComparison.OrdinalIgnoreCase))
                    {
                        //TODO: botteling step, beta shit.
                    }
                }
            }

            if (source.Yeasts != null)
            {
                foreach (var yeast in source.Yeasts)
                {
                    var yeastStepDto = GetYeastStepDto(yeast);
                    var fermentationSteps = recipeDto.Steps.OfType<FermentationStepDto>();
                    var inSecondary = string.Equals(yeast.Add_To_Secondary, "false", StringComparison.OrdinalIgnoreCase);
                    if (!inSecondary)
                    {
                       
                        var fermentationStepDto = (fermentationSteps.FirstOrDefault() ?? GetFermentationStepDto(recipeDto, 0)) ?? fermentationSteps.FirstOrDefault();
                        fermentationStepDto.Ingredients.Add(yeastStepDto);
                    }
                    else
                    {
                        var fermentationStepDto = (fermentationSteps.Skip(1).FirstOrDefault() ?? GetFermentationStepDto(recipeDto, 0)) ?? fermentationSteps.FirstOrDefault();
                        fermentationStepDto.Ingredients.Add(yeastStepDto);
                    }

                }
            }

            SetStepNumber(recipeDto);

            return recipeDto;
            }
            catch(Exception)
            {
                throw;
            }
        }

        

        private void SetStepNumber(RecipeDto recipeDto)
        {
            var stepNumber = 1;
            foreach (var item in recipeDto.Steps.OfType<MashStepDto>())
            {
                var mashStep = item;
                mashStep.StepNumber = stepNumber;
                stepNumber++;
            }
            //TODO: SPARGE STEP
            //if (recipeDto.Steps.All(s => s.Type != "sparge"))
            //{
            //    recipeDto.SpargeStep.StepNumber = stepNumber;
            //    stepNumber++;
            //}

            foreach (var boilStep in recipeDto.Steps.OfType<BoilStepDto>())
            {
                boilStep.StepNumber = stepNumber;
                stepNumber++;
            }

            foreach (var item in recipeDto.Steps.OfType<FermentationStepDto>())
            {
                var fermentationStep = item;
                fermentationStep.StepNumber = stepNumber;
                stepNumber++;
            }
        }


        private static FermentationStepDto GetFermentationStepDto(RecipeDto recipeDto, int time)
        {
            var fermentationSteps = recipeDto.Steps.OfType<FermentationStepDto>();
            var fermentationStep = fermentationSteps?.FirstOrDefault(f => f.Length == time / 24);
            return fermentationStep != null ? fermentationStep : fermentationSteps?.Select(f => f).FirstOrDefault();
        }

        private static MashStepDto GetMashStepDto(Model.BeerXml.MashStep mashStep)
        {
            var mashStepDto = new MashStepDto();
            mashStepDto.Length = (mashStep.StepTime.Any()) ? decimal.Parse(mashStep.StepTime, CultureInfo.InvariantCulture) : 0;
            //TODO: make double???
            mashStepDto.Temperature = (int)double.Parse(mashStep.StepTemp, CultureInfo.InvariantCulture);
            mashStepDto.Notes = mashStep.Name;
            mashStepDto.Ingredients = new List<IIngredientStepDto>();
            return mashStepDto;
        }

        private static MashStepDto GetMashStepDto(RecipeDto recipeDto, int time)
        {
            var mashStepsDto = recipeDto.Steps.OfType<MashStepDto>();
            var mashStepDto = mashStepsDto.Select(c =>c).FirstOrDefault(m => m.Length == time);
            return mashStepDto;
        }

        private static BoilStepDto GetBoilStepDto(RecipeDto recipeDto, int time)
        {
            var boilSteps = recipeDto.Steps.OfType<BoilStepDto>();
            var boilStep = boilSteps?.FirstOrDefault(b => b.Length == time);
            if (boilStep != null) return boilStep;
            boilStep = new BoilStepDto
            {
                Length = time,
                Ingredients = new List<IIngredientStepDto>(),
            };
            recipeDto.Steps.Add(boilStep);
            return boilStep;
        }

        private FermentableStepDto GetFermentableStepDto(Model.BeerXml.Fermentable fermentable, MashStepDto mashStep)
        {
            var fermentableDto = _fermentableElasticsearch.Search(fermentable.Name, 0, 1).FirstOrDefault();
            if (fermentableDto == null)
            {
                {  
                    return null;
                };
            }
            var fermentableStepDto = AutoMapper.Mapper.Map<FermentableDto, FermentableStepDto>(fermentableDto);
            double amount = double.Parse(fermentable.Amount, CultureInfo.InvariantCulture);
            fermentableStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            return fermentableStepDto;
        }

        private FermentableStepDto GetFermentableStepDto(Model.BeerXml.Fermentable fermentable, FermentationStepDto fermentationStep)
        {
            var fermentableDto = _fermentableElasticsearch.Search(fermentable.Name, 0, 1).FirstOrDefault();
            if (fermentableDto == null)
            {
                {
                    return null;
                };
            }
            var fermentableStepDto = AutoMapper.Mapper.Map<FermentableDto, FermentableStepDto>(fermentableDto);
            double amount = double.Parse(fermentable.Amount, CultureInfo.InvariantCulture);
            fermentableStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            return fermentableStepDto;
        }


        private HopStepDto GetHopStepDto(Model.BeerXml.Hop hop)
        {
            var hopForms = _hopRepository.GetHopFormsAsync().Result;
            var hopDto = _hopElasticsearch.Search(hop.Name, 0, 1).FirstOrDefault();
            if (hopDto == null)
            {
                {
                    return null;
                };

            };
            var hopStepDto = AutoMapper.Mapper.Map<HopDto, HopStepDto>(hopDto);
            hopStepDto.HopId = hopDto.Id;
            double alpha = double.Parse(hop.Alpha, CultureInfo.InvariantCulture);
            hopStepDto.AAValue = alpha;
            double amount = double.Parse(hop.Amount, CultureInfo.InvariantCulture);
            hopStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            var hopForm =
                hopForms.FirstOrDefault(h => string.Equals(h.Name, hop.Form, StringComparison.OrdinalIgnoreCase)) ??
                hopForms.First();
            hopStepDto.SubType = hopForm.Name;
            return hopStepDto;
        }

        private OtherStepDto GetOthersStepDto(Model.BeerXml.Misc misc)
        {
            var otherDto = _otherElasticsearch.Search(misc.Name, 0, 1).FirstOrDefault();
            if (otherDto == null)
            {
                otherDto = new OtherDto
                {
                    Name = misc.Name,
                    Custom = true,
                };
                //otherDto = _otherService.AddAsync(otherDto).Result;
            };
            var otherStepDto = AutoMapper.Mapper.Map<OtherDto, OtherStepDto>(otherDto);
            double amount = string.IsNullOrEmpty(misc.Amount) ? 0 : double.Parse(misc.Amount, CultureInfo.InvariantCulture);
            otherStepDto.Amount = (int)Math.Round(amount * 1000, 0);
            return otherStepDto;
        }

        private YeastStepDto GetYeastStepDto(Model.BeerXml.Yeast yeast)
        {
            var yeastDto = _yeastElasticsearch.SearchAsync(yeast.Name, 0, 1).Result.FirstOrDefault();
            if (yeastDto == null)
            {
                return null;
            };
            var yeastStepDto = AutoMapper.Mapper.Map<YeastDto, YeastStepDto>(yeastDto);

            double amount = (yeast.Amount != null) ? double.Parse(yeast.Amount, CultureInfo.InvariantCulture) : 0;
            yeastStepDto.Amount = (int)Math.Round(amount, 0);
            return yeastStepDto;
        }
    }
}
