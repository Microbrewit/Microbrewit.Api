using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("FERMENTABLE")]
    public class Fermentable
    {
        [XmlElement("NAME")]
        public string Name { get; set; }
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [XmlElement("TYPE")]
        public string Type { get; set; }
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        [XmlElement("YIELD")]
        public string Yield { get; set; }
        [XmlElement("COLOR")]
        public string Color { get; set; }
        
        [XmlElement("ADD_AFTER_BOIL")]
        public string AddAfterBoil { get; set; }
        
        [XmlElement("ORIGIN")]
        public string Origin { get; set; }
        
        [XmlElement("SUPPLIER")]
        public string Supplier { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        // These should be double
        
        [XmlElement("COARSE_FINE_DIFF")]
        public string Coarse_Fine_Diff { get; set; }
        
        [XmlElement("MOISTURE")]
        public string Moisture { get; set; }
        
        [XmlElement("DIASTATIC_POWER")]
        public string DiastaticPower { get; set; }
        
        [XmlElement("PROTEIN")]
        public string Protein { get; set; }
        
        [XmlElement("MAX_IN_BATCH")]
        public string Max_In_Batch { get; set; }
        
        [XmlElement("RECOMMEND_MASH")]
        public string RecommendMash { get; set; }
        
        [XmlElement("IBU_GAL_PER_LB")]
        public string Ibu_Gal_Per_Lb { get; set; }
        
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
        
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        
        [XmlElement("POTENTIAL")]
        public string Potential { get; set; }
        
        [XmlElement("DISPLAY_COLOR")]
        public string Display_Color { get; set; }
    }
}
