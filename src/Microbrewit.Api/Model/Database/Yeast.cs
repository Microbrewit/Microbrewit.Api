using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class Yeast
    {
        public int YeastId { get; set; }
        public string Name { get; set; }
        public double TemperatureHigh { get; set; }
        public double TemperatureLow { get; set; }
        public string Flocculation { get; set; }
        public int FlocculationLow { get; set; }
        public int FlocculationHigh { get; set; }
        public string AlcoholTolerance { get; set; }
        public double AlcoholToleranceLow { get; set; }
        public double AlcoholToleranceHigh { get; set; }
        public string ProductCode { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        // new from google doc.
        public string BrewerySource { get; set; }
        public string Species { get; set; }
        public string AttenutionRange { get; set; }
        public int AttenutionLow { get; set; }
        public int AttenutionHigh { get; set; }
        public string PitchingFermentationNotes { get; set; }
       
        // relations
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public bool Custom { get; set; }
        public IEnumerable<Source> Sources { get; set; }
        public ICollection<FermentationStepYeast> FermentationSteps { get; set; }
    }
}
