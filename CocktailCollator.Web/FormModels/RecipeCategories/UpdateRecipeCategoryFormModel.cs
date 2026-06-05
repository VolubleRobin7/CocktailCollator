using AutoMapper;
using CocktailCollator.Application.UseCases.RecipeCategories.UpdateRecipeCategory;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.RecipeCategories;

public class UpdateRecipeCategoryFormModel : IFormModel<UpdateRecipeCategoryInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<Guid> RecipeCategoryId { get; set; }
        = new(() => Guid.Empty, (input) => input != Guid.Empty);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public UpdateRecipeCategoryFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public UpdateRecipeCategoryInputPort ExtractToInputPort()
        => this._mapper.Map<UpdateRecipeCategoryInputPort>(this);

    public bool IsValid()
        => this.RecipeCategoryId.IsValid() && this.Name.IsValid();

    public void ResetToDefault()
    {
        this.RecipeCategoryId.ResetToDefault();
        this.Name.ResetToDefault();
    }
}
