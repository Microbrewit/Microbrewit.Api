using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper
{
    public interface IBeerXmlResolver
    {
         RecipeDto ResolveCore(Model.BeerXml.Recipe source);
    }
}