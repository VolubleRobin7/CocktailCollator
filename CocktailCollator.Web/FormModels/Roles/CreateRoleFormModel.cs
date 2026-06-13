using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using System.Collections.ObjectModel;

namespace CocktailCollator.Web.FormModels.Roles;

public class CreateRoleFormModel : IFormModel<CreateRoleInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<ObservableCollection<string>> Claims { get; set; }
        = new(() => [], input => true);
    public InputProperty<bool> HasEveryClaim { get; set; }
        = new(() => false, input => true);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, input => !string.IsNullOrWhiteSpace(input));

    public Action? OnChange { get; set; }

    public CreateRoleFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Claims.OnChange = () => OnChange?.Invoke();
        this.Claims.Input.CollectionChanged += (_, _) => OnChange?.Invoke();
        this.HasEveryClaim.OnChange = () => OnChange?.Invoke();
        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public CreateRoleInputPort ExtractToInputPort()
        => this._mapper.Map<CreateRoleInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid() && (this.HasEveryClaim.Input || this.Claims.Input.Count > 0);

    public void ResetToDefault()
    {
        this.Name.ResetToDefault();
        this.Claims.ResetToDefault();
        this.HasEveryClaim.ResetToDefault();
    }
}
