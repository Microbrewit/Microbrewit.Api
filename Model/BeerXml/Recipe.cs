using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("RECIPE")]
    public class Recipe
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement(ElementName = "VERSION")]
        public string Version { get; set; }
        
        [XmlElement("TYPE")]
        public string Type { get; set; }
        
        [XmlElement("BREWER")]
        public string Brewer { get; set; }
        
        [XmlElement("ASST_BREWER")]
        public string Asst_Brewer { get; set; }
        
        [XmlElement("BATCH_SIZE")]
        public string BatchSize { get; set; }
        
        [XmlElement("BOIL_SIZE")]
        public string BoilSize { get; set; }
        
        [XmlElement("BOIL_TIME")]
        public string Boil_Time { get; set; }
        
        [XmlElement("EFFICIENCY")]
        public string Efficiency { get; set; }
        
        [XmlArray("HOPS")]
        [XmlArrayItem("HOP")]
        public List<Hop> Hops { get; set; }
        
        [XmlArray("FERMENTABLES")]
        [XmlArrayItem("FERMENTABLE")]
        public List<Fermentable> Fermentables { get; set; }
        
        [XmlArray("MISCS")]
        [XmlArrayItem("MISC")]
        public List<Misc> Miscs { get; set; }
        
        [XmlArray("YEASTS")]
        [XmlArrayItem("YEAST")]
        public List<Yeast> Yeasts { get; set; }
        
        [XmlArray("WATERS")]
        [XmlArrayItem("WATER")]
        public List<Water> Waters { get; set; }
        
        [XmlElement("STYLE")]
        public Style Style { get; set; }
        
        [XmlElement("EQUIPMENT")]
        public Equipment Equipment { get; set; }
        
        [XmlElement("MASH")]
        public Mash Mash { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("TASTE_NOTES")]
        public string Taste_Notes { get; set; }
        
        [XmlElement("TASTE_RATING")]
        public string Taste_Rating { get; set; }
        
        [XmlElement("OG")]
        public string Og { get; set; }
        
        [XmlElement("FG")]
        public string Fg { get; set; }
        
        [XmlElement("CARBONATION")]
        public string Carbonation { get; set; }
        
        [XmlElement("FERMENTATION_STAGES")]
        public string Fermentation_Stages { get; set; }
        
        [XmlElement("PRIMARY_AGE")]
        public string PrimaryAge { get; set; }
        
        [XmlElement("PRIMARY_TEMP")]
        public string Primary_Temp { get; set; }
        
        [XmlElement("SECONDARY_AGE")]
        public string Secondary_Age { get; set; }
        
        [XmlElement("SECONDARY_TEMP")]
        public string Secondary_Temp { get; set; }
        
        [XmlElement("TERTIARY_AGE")]
        public string TertiaryAge { get; set; }
        
        [XmlElement("TERTIARY_TEMP")]
        public string TertiaryTemp { get; set; }
        
        [XmlElement("AGE")]
        public string Age { get; set; }
        
        [XmlElement("AGE_TEMP")]
        public string Age_Temp { get; set; }
        
        [XmlElement("CARBONATION_USED")]
        public string Carbonation_Used { get; set; }
        
        [XmlElement("DATE")]
        public string Date { get; set; }
        
        [XmlElement("EST_OG")]
        public string Est_Og { get; set; }
        
        [XmlElement("EST_FG")]
        public string Est_Fg { get; set; }
        
        [XmlElement("EST_COLOR")]
        public string Est_Color { get; set; }
        
        [XmlElement("IBU")]
        public string Ibu { get; set; }
        
        [XmlElement("IBU_METHOD")]
        public string Ibu_Method { get; set; }
        
        [XmlElement("EST_ABV")]
        public string Est_Abv { get; set; }
        
        [XmlElement("ABV")]
        public string Abv { get; set; }
        
        [XmlElement("ACTUAL_EFFICIENCY")]
        public string Actual_Efficiency { get; set; }
        
        [XmlElement("CALORIES")]
        public string Calories { get; set; }
        
        [XmlElement("DISPLAY_BATCH_SIZE")]
        public string Display_Batch_Size { get; set; }
        
        [XmlElement("DISPLAY_BOIL_SIZE")]
        public string Display_Boil_Size { get; set; }
        
        [XmlElement("DISPLAY_OG")]
        public string Display_Og { get; set; }
        
        [XmlElement("DISPLAY_FG")]
        public string Display_Fg { get; set; }
        
        [XmlElement("DISPLAY_PRIMARY_TEMP")]
        public string Display_Primary_Temp { get; set; }
        
        [XmlElement("DISPLAY_SECONDARY_TEMP")]
        public string Display_Secondary_Temp { get; set; }
        
        [XmlElement("DISPLAY_TERTIARY_TEMP")]
        public string Display_Tertiary_Temp { get; set; }
        
        [XmlElement("DISPLAY_AGE_TEMP")]
        public string Display_Age_Temp { get; set; }
    }
}
