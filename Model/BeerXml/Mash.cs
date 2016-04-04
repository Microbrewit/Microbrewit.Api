using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("MASH")]
    public class Mash
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERSION")]
        public string Version { get; set; }
        
        [XmlElement("GRAIN_TEMP")]
        public string GrainTemp { get; set; }
        
        [XmlElement("TUN_TEMP")]
        public string TunTemp { get; set; }
        
        [XmlElement("SPARGE_TEMP")]
        public string SpargeTemp { get; set; }
        
        [XmlElement("PH")]
        public string Ph { get; set; }
        
        [XmlElement("TUN_WEIGHT")]
        public string TunWeight { get; set; }
        
        [XmlElement("TUN_SPECIFIC_HEAT")]
        public string TunSpecificHeat { get; set; }
        
        [XmlElement("EQUIP_ADJUST")]
        public string EquipAdjust { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("DISPLAY_GRAIN_TEMP")]
        public string DisplayGrainTemp { get; set; }
        
        [XmlElement("DISPLAY_TUN_TEMP")]
        public string DisplayTunTemp { get; set; }
        
        [XmlElement("DISPLAY_SPARGE_TEMP")]
        public string DisplaySpargeTemp { get; set; }
        
        [XmlElement("DISPLAY_TUN_WEIGHT")]
        public string DisplayTunWeight { get; set; }
        
        [XmlArray("MASH_STEPS")]
        [XmlArrayItem("MASH_STEP")]
        public List<MashStep> MashSteps { get; set; }
    }
}
