using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("HOP")]
    public class Hop
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERSION")]
        public string Version { get; set; }
        
        [XmlElement("ORIGIN")]
        public string Origin { get; set; }
        
        [XmlElement("ALPHA")]
        public string Alpha { get; set; }
        
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        
        [XmlElement("USE")]
        public string Use { get; set; }
        
        [XmlElement("TIME")]
        public string Time { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("TYPE")]
        public string Type { get; set; }
        
        [XmlElement("FORM")]
        public string Form { get; set; }
        
        [XmlElement("BETA")]
        public string Beta { get; set; }
        
        [XmlElement("HSI")]
        public string HSI { get; set; }
        
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
        
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        
        [XmlElement("DISPLAY_TIME")]
        public string Display_Time { get; set; }
    }
}
