using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class Other
    {
        public int OtherId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Custom { get; set; }

        public ICollection<MashStepOther> MashSteps { get; set; }
        public ICollection<BoilStepOther> BoilSteps { get; set; }
        public ICollection<FermentationStepOther> FermentationSteps { get; set; }

       

    }
}
