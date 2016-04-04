using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("STYLE")]
    public class Style
    {
        
        [XmlElement("NAME")]
        public string Name { get; set; }
        
        [XmlElement("VERSION")]
        public string Version { get; set; }
        
        [XmlElement("CATEGORY")]
        public string Category { get; set; }
        
        [XmlElement("CATEGORY_NUMBER")]
        public string CategoryNumber { get; set; }
        
        [XmlElement("STYLE_LETTER")]
        public string StyleLetter { get; set; }
        
        [XmlElement("STYLE_GUIDE")]
        public string StyleGuide { get; set; }
        
        [XmlElement("TYPE")]
        public string Type { get; set; }
        
        [XmlElement("OG_MIN")]
        public string OgMin { get; set; }
        
        [XmlElement("OG_MAX")]
        public string OgMax { get; set; }
        
        [XmlElement("FG_MIN")]
        public string FgMin { get; set; }
        
        [XmlElement("FG_MAX")]
        public string FgMax { get; set; }
        
        [XmlElement("IBU_MIN")]
        public string IbuMin { get; set; }
        
        [XmlElement("IBU_MAX")]
        public string IbuMax { get; set; }
        
        [XmlElement("COLOR_MIN")]
        public string ColorMin { get; set; }
        
        [XmlElement("COLOR_MAX")]
        public string ColorMax { get; set; }
        
        [XmlElement("CARB_MIN")]
        public string CarbMin { get; set; }
        
        [XmlElement("CRB_MAX")]
        public string CarbMax { get; set; }
        
        [XmlElement("ABV_MIN")]
        public string AbvMin { get; set; }
        
        [XmlElement("ABV_MAX")]
        public string AbvMax { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("PROFILE")]
        public string Profile { get; set; }
        
        [XmlElement("INGREDIENTS")]
        public string Ingredients { get; set; }
        
        [XmlElement("DISPLAY_OG_MIN")]
        public string DisplayOgMin { get; set; }
        
        [XmlElement("DISPLAY_OG_MAX")]
        public string DisplayOgMax { get; set; }
        
        [XmlElement("DISPLAY_FG_MIN")]
        public string DisplayFgMin { get; set; }
        
        [XmlElement("DISPLAY_FG_MAX")]
        public string DisplayFgMax { get; set; }
        
        [XmlElement("DISPLAY_COLOR_MIN")]
        public string DisplayColorMin { get; set; }
        
        [XmlElement("DISPLAY_COLOR_MAX")]
        public string DisplayColorMax { get; set; }
        
        [XmlElement("OG_RANGE")]
        public string OgRange { get; set; }
        
        [XmlElement("FG_RANGE")]
        public string FgRange { get; set; }
        
        [XmlElement("IBU_RANGE")]
        public string IbuRange { get; set; }
        
        [XmlElement("DISPLAY_CARB_RANGE")]
        public string CarbRange { get; set; }
        
        [XmlElement("DISPLAY_COLOR_RANGE")]
        public string ColorRange { get; set; }
        
        [XmlElement("DISPLAY_ABV_RANGE")]
        public string AbvRange { get; set; }
    }
}
