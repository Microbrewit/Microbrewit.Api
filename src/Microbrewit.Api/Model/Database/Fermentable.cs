using System;
using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class Fermentable
    {
        
        public int FermentableId { get; set; }
        public string Name { get; set; }
        public double EBC { get; set; }
        public double Lovibond { get; set; }
        public string Note { get; set; }
        public int? PPG { get; set; }
        public int? SupplierId { get; set; }
        public string Type { get; set; }
        public bool Custom { get; set; }
        public bool MustMash { get; set; }
        public int MaxInBatch { get; set; }
        public decimal Protein { get; set; }
        public decimal DiastaticPower { get; set; }
        public bool AddAfterBoil { get; set; }
        public decimal Moisture { get; set; }
        public int? SuperFermentableId { get; set; }
        public Fermentable SuperFermentable { get; set; }
        public ICollection<Fermentable> SubFermentables { get; set; }
      
        public Supplier Supplier { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public IEnumerable<Flavour> Flavours { get; set; }
        public ICollection<MashStepFermentable> MashSteps { get; set; }
        public ICollection<BoilStepFermentable> BoilSteps { get; set; }
        public ICollection<FermentationStepFermentable> FermentationSteps { get; set; }
        public IEnumerable<Source> Sources { get; set; }
    }
}