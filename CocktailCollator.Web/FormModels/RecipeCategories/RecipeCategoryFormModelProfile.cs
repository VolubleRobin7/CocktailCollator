using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;
using CocktailCollator.Application.UseCases.RecipeCategories.UpdateRecipeCategory;

namespace CocktailCollator.Web.FormModels.RecipeCategories;

public class RecipeCategoryFormModelProfile : Profile
{
    public RecipeCategoryFormModelProfile()
    {
        _ = this.CreateMap<CreateRecipeCategoryFormModel, CreateRecipeCategoryInputPort>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Input));

        _ = this.CreateMap<UpdateRecipeCategoryFormModel, UpdateRecipeCategoryInputPort>()
            .ForMember(dest => dest.RecipeCategoryId, opt => opt.MapFrom(src => src.RecipeCategoryId.Input))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Input));
    }
}
