using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

namespace CocktailCollator.Web.FormModels.Recipes;

public class RecipeFormModelProfile : Profile
{
    public RecipeFormModelProfile()
    {
        // automapper needs further research into generics to avoid this sillyness
        _ = this.CreateMap<CreateRecipeFormModel, CreateRecipeInputPort>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps.Input))
            .ForMember(d => d.Ingredients, o => o.MapFrom(s => s.Ingredients.Input));

        _ = this.CreateMap<CreateRecipeFormModelIngredient, CreateRecipeInputPortIngredient>()
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.Input))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.Measurement, o => o.MapFrom(s => s.Measurement.Input));

        _ = this.CreateMap<CreateRecipeFormModelMeasurement, CreateRecipeInputPortMeasurement>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input));

        _ = this.CreateMap<CreateRecipeFormModelStep, CreateRecipeInputPortStep>()
            .ForMember(d => d.Instruction, o => o.MapFrom(s => s.Instruction.Input))
            .ForMember(d => d.Order, o => o.MapFrom(s => s.Order.Input));
    }
}
