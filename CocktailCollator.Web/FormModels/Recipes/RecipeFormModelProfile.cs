using AutoMapper;
using CocktailCollator.Application.UseCases.Recipes.CreateRecipe;
using CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

namespace CocktailCollator.Web.FormModels.Recipes;

public class RecipeFormModelProfile : Profile
{
    public RecipeFormModelProfile()
    {
        _ = this.CreateMap<CreateRecipeFormModel, CreateRecipeInputPort>();

        _ = this.CreateMap<RecipeFormModelIngredient, CreateRecipeInputPortRecipeIngredient>()
            .ForMember(d => d.Ingredient, o => o.MapFrom(s => s.UsingExistingIngredient ? null : s))
            .ForMember(d => d.IngredientId, o => o.MapFrom(s => s.UsingExistingIngredient ? s.ExistingIngredientId : Guid.Empty))
            .ForMember(d => d.MeasurementId, o => o.MapFrom(s => s.Measurement));

        _ = this.CreateMap<RecipeFormModelIngredient, CreateRecipeInputPortIngredient>();

        _ = this.CreateMap<RecipeFormModelStep, CreateRecipeInputPortStep>();

        _ = this.CreateMap<UpdateRecipeFormModel, UpdateRecipeInputPort>()
            .ForMember(d => d.RecipeCategoryId, o => o.Ignore());

        _ = this.CreateMap<RecipeFormModelIngredient, UpdateRecipeInputPortRecipeIngredient>()
            .ForMember(d => d.Ingredient, o => o.MapFrom(s => s.UsingExistingIngredient ? null : s))
            .ForMember(d => d.IngredientId, o => o.MapFrom(s => s.UsingExistingIngredient ? s.ExistingIngredientId : Guid.Empty))
            .ForMember(d => d.MeasurementId, o => o.MapFrom(s => s.Measurement));

        _ = this.CreateMap<RecipeFormModelIngredient, UpdateRecipeInputPortIngredient>();

        _ = this.CreateMap<RecipeFormModelStep, UpdateRecipeInputPortStep>();
    }
}
