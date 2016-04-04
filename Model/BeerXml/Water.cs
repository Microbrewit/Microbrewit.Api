using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("WATER")]
    public class Water
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERION")]
        public string Version { get; set; }
        
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        
        [XmlElement("CALCIUM")]
        public string Calcium { get; set; }
        
        [XmlElement("BICRBONATE")]
        public string Bicarbonate { get; set; }
        
        [XmlElement("SULFATE")]
        public string Sulfate { get; set; }
        
        [XmlElement("CHLORIDE")]
        public string Chloride { get; set; }
        
        [XmlElement("SODIUM")]
        public string Sodium { get; set; }
        
        [XmlElement("MAGNESIUM")]
        public string Magnesium { get; set; }
        
        [XmlElement("PH")]
        public string Ph { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
    }
}
