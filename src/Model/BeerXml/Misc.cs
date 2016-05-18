using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("MISC")]
    public class Misc
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERSION")]
        public string Version { get; set; }
        
        [XmlElement("TYPE")]
        public string Type { get; set; }
        
        [XmlElement("USE")]
        public string Use { get; set; }
        
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        
        [XmlElement("TIME")]
        public string Time { get; set; }
        
        [XmlElement("AMOUNT_IS_WEIGHT")]
        public string AmountIsWeight { get; set; }
        
        [XmlElement("USE_FOR")]
        public string UseFor { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("DISPLAY_AMOUNT")]
        public string DisplayAmount { get; set; }
        
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        
        [XmlElement("DISPLAY_TIME")]
        public string DisplayTime { get; set; }
        
        [XmlElement("BATCH_SIZE")]
        public string BatchSize { get; set; }
    }
}
