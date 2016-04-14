using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("EQUIPMENT")]
    public class Equipment
    {
        [XmlElement("NAME")]
        public string Name { get; set; }
        [XmlElement("VERSION")]
        public string Version { get; set; }
        [XmlElement("BOIL_SIZE")]
        public string Boil_Size { get; set; }
        [XmlElement("BATCH_SIZE")]
        public string Batch_Size { get; set; }
        [XmlElement("TUN_VOLUME")]
        public string Tun_Volume { get; set; }
        [XmlElement("TUN_WEIGHT")]
        public string Tun_Weight { get; set; }
        [XmlElement("TUN_SPECIFIC_HEAT")]
        public string Tun_Specific_Heat { get; set; }
        [XmlElement("TOP_UP_WATER")]
        public string Top_Up_Water { get; set; }
        [XmlElement("TRUB_CHILLER_LOSS")]
        public string Trub_Chiller_Loss { get; set; }
        [XmlElement("EVAP_RATE")]
        public string Evap_Rate { get; set; }
        [XmlElement("BOIL_TIME")]
        public string Boil_Time { get; set; }
        [XmlElement("CALC_BOIL_VOLUME")]
        public string Calc_Boil_Volume { get; set; }
        [XmlElement("LAUTER_DEADSPACE")]
        public string Lauter_Deadspace { get; set; }
        
        [XmlElement("TOP_UP_KETTLE")]
        public string Top_Up_Kettle { get; set; }
        
        [XmlElement("HOP_UTILIZATION")]
        public string Hop_Utilization { get; set; }
        
        [XmlElement("NOTES")]
        public string Notes { get; set; }
        
        [XmlElement("DISPLAY_BOIL_SIZE")]
        public string Display_Boil_Size { get; set; }
        
        [XmlElement("DISPLAY_BATCH_SIZE")]
        public string Display_Batch_Size { get; set; }
        
        [XmlElement("DISPLAY_TUN_VOLUME")]
        public string Display_Tun_Volume { get; set; }
        
        [XmlElement("DISPLAY_TOP_UP_WEIGHT")]
        public string Display_Tun_Weight { get; set; }
        
        [XmlElement("DISPLAY_TOP_UP_WATER")]
        public string Display_TopUp_Water { get; set; }
        
        [XmlElement("DISPLAY_TRUB_CHILLER_LOSS")]
        public string Display_Trub_Chiller_Loss { get; set; }
        
        [XmlElement("DISPLAY_LAUTER_DEADSPACE")]
        public string Display_Lauter_Deadspace { get; set; }
        
        [XmlElement("DISPLAY_TOP_UP_KETTLE")]
        public string Display_Top_Up_Kettle { get; set; }
    }
}
