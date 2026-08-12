using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

namespace CocktailCollator.Web.FormModels.Recipes;

public class RecipeFormModelProfile : Profile
{
    public RecipeFormModelProfile()
    {
        // automapper needs further research into generics to avoid this sillyness
        _ = this.CreateMap<CreateRecipeFormModel, CreateRecipeInputPort>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps.Select(step => step.Input)))
            .ForMember(d => d.Ingredients, o => o.MapFrom(s => s.Ingredients.Input))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.Where(i => i.Output != null)));

        _ = this.CreateMap<CreateRecipeFormModelIngredient, CreateRecipeInputPortRecipeIngredient>()
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.Input))
            .ForMember(d => d.Ingredient, o => o.MapFrom(s => s.UsingExistingIngredient.Input ? null : s))
            .ForMember(d => d.IngredientId, o => o.MapFrom(s => s.UsingExistingIngredient.Input ? s.ExistingIngredientId : Guid.Empty))
            .ForMember(d => d.MeasurementId, o => o.MapFrom(s => s.Measurement.Input));

        _ = this.CreateMap<CreateRecipeFormModelIngredient, CreateRecipeInputPortIngredient>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input));

        _ = this.CreateMap<CreateRecipeFormModelStep, CreateRecipeInputPortStep>()
            .ForMember(d => d.Instruction, o => o.MapFrom(s => s.Instruction.Input))
            .ForMember(d => d.Order, o => o.MapFrom(s => s.Order.Input));

        _ = this.CreateMap<UpdateRecipeFormModel, UpdateRecipeInputPort>()
            .ForMember(d => d.RecipeId, o => o.MapFrom(s => s.RecipeId.Input))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.RecipeCategoryId, o => o.Ignore())
            .ForMember(d => d.Ingredients, o => o.MapFrom(s => s.Ingredients.Input))
            .ForMember(d => d.Steps, o => o.MapFrom(s => s.Steps.Input));

        _ = this.CreateMap<UpdateRecipeFormModelIngredient, UpdateRecipeInputPortRecipeIngredient>()
            .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount.Input))
            .ForMember(d => d.Ingredient, o => o.MapFrom(s => s.UsingExistingIngredient.Input ? null : s))
            .ForMember(d => d.IngredientId, o => o.MapFrom(s => s.UsingExistingIngredient.Input ? s.ExistingIngredientId : Guid.Empty))
            .ForMember(d => d.MeasurementId, o => o.MapFrom(s => s.Measurement.Input));

        _ = this.CreateMap<UpdateRecipeFormModelIngredient, UpdateRecipeInputPortIngredient>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input));

        _ = this.CreateMap<UpdateRecipeFormModelStep, UpdateRecipeInputPortStep>()
            .ForMember(d => d.Instruction, o => o.MapFrom(s => s.Instruction.Input))
            .ForMember(d => d.Order, o => o.MapFrom(s => s.Order.Input));
    }
}
