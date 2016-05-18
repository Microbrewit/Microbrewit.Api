using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microbrewit.Api.Model.BeerXml
{
    [XmlRoot("RECIPES")]
    public class RecipesComplete
    {
       [XmlElement("RECIPE")]
       public List<Recipe> Recipes { get; set; }
    }
}
