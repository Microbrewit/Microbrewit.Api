using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("MASH_STEP")]
    public class MashStep
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERISON")]
        public string Version { get; set; }
        
        [XmlElement("TYPE")]
        public string Type { get; set; }
        
        [XmlElement("INFUSE_AMOUNT")]
        public string InfuseAmount { get; set; }
        
        [XmlElement("STEP_TIME")]
        public string StepTime { get; set; }
        
        [XmlElement("STEP_TEMP")]
        public string StepTemp { get; set; }
        
        [XmlElement("RAMP_TIME")]
        public string RampTime { get; set; }
        
        [XmlElement("END_TEMP")]
        public string EndTemp { get; set; }
        
        [XmlElement("DESCRIPTION")]
        public string Description { get; set; }
        
        [XmlElement("WATER_GRAIN_RATIO")]
        public string WaterGrainRatio { get; set; }
        
        [XmlElement("DECOCTION_AMT")]
        public string DecoctionAmt { get; set; }
        
        [XmlElement("INFUSE_TEMP")]
        public string InfuseTemp { get; set; }
        
        [XmlElement("DISPLAY_STEP_TEMP")]
        public string DisplayStepTemp { get; set; }
        
        [XmlElement("DISPLAY_INFUSE_AMT")]
        public string DisplayInfuseAmt { get; set; }
    }
}
