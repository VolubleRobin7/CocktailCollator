using AutoMapper;
using CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

namespace CocktailCollator.Web.FormModels.IngredientCategories;

public class IngredientCategoryFormModelProfile : Profile
{
    public IngredientCategoryFormModelProfile()
    {
        _ = this.CreateMap<CreateIngredientCategoryFormModel, CreateIngredientCategoryInputPort>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input));
    }
}
