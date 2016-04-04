using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("YEAST")]
    public class Yeast
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERSION")]
        public string Version { get; set; }
        
        [XmlElement("TYPE")]
        public string Type { get; set; }
        
        [XmlElement("FORM")]
        public string Form { get; set; }
        
        [XmlElement("AMOUNT")]
        public string Amount { get; set; }
        
        [XmlElement("AMOUNT_IS_WEIGHT")]
        public string Amount_Is_Weight { get; set; }
        
        [XmlElement("LABORATORY")]
        public string Laboratory { get; set; }
        
        [XmlElement("PRODUCT_ID")]
        public string Product_Id { get; set; }
        
        [XmlElement("MIN_TEMPERATURE")]
        public string Min_Temperature { get; set; }
        
        [XmlElement("MAX_TEMPERATURE")]
        public string Max_Temperature { get; set; }
        
        [XmlElement("FLOCCULATION")]
        public string Flocculation { get; set; }
        
        [XmlElement("ATTENUATION")]
        public string Attenuation { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("BEST_FOR")]
        public string Best_For { get; set; }
        
        [XmlElement("MAZ_REUSE")]
        public string Max_Reuse { get; set; }
        
        [XmlElement("TIMES_CULTURED")]
        public string Times_Cultured { get; set; }
        
        [XmlElement("ADD_TO_SECONDARY")]
        public string Add_To_Secondary { get; set; }
        
        [XmlElement("DISPLAY_AMOUNT")]
        public string Display_Amount { get; set; }
        
        [XmlElement("DISPLAY_MIN_TEMP")]
        public string Display_Min_Temp { get; set; }
        
        [XmlElement("DISPLAY_MAX_TEMP")]
        public string Display_Max_Temp { get; set; }
        
        [XmlElement("INVENTORY")]
        public string Inventory { get; set; }
        
        [XmlElement("CULTURE_DATE")]
        public string Culture_Date { get; set; }
    }
}
