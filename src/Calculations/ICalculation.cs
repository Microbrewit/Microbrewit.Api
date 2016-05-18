using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Calculations
{
    public interface ICalculation
    {
        SRM CalculateSRM(Recipe recipe);
        SRMDto CalculateSRMDto(RecipeDto recipe);
        IBU CalculateIBU(Recipe recipe);
        IBUDto CalculateIBUDto(RecipeDto recipe);
        double CalculateOG(Recipe recipe);
        double CalculateOGDto(RecipeDto recipe);
        ABV CalculateABV(Recipe recipe);
        ABVDto CalculateABVDto(RecipeDto recipe);
        
    }
}