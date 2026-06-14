using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using System.Collections.ObjectModel;

namespace CocktailCollator.Web.FormModels.Roles;

public class UpdateRoleFormModel : IFormModel<UpdateRoleInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<ObservableCollection<string>> Claims { get; set; }
    public InputProperty<bool> HasEveryClaim { get; set; }
        = new(() => false, input => true);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, input => !string.IsNullOrWhiteSpace(input));
    public InputProperty<Guid> RoleId { get; set; }
        = new(() => Guid.Empty, input => input != Guid.Empty);

    public Action? OnChange { get; set; }

    public UpdateRoleFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Claims = new(() => [], this.IsClaimsValid)
        {
            OnChange = () => OnChange?.Invoke()
        };
        this.Claims.Input.CollectionChanged += (_, _) => OnChange?.Invoke();
        this.HasEveryClaim.OnChange = () => OnChange?.Invoke();
        this.Name.OnChange = () => OnChange?.Invoke();
        this.RoleId.OnChange = () => OnChange?.Invoke();
    }

    public UpdateRoleInputPort ExtractToInputPort()
        => this._mapper.Map<UpdateRoleInputPort>(this);

    public bool IsValid()
        => this.RoleId.IsValid() && this.Name.IsValid() && this.Claims.IsValid();

    public void ResetToDefault()
    {
        this.RoleId.ResetToDefault();
        this.Name.ResetToDefault();
        this.Claims.ResetToDefault();
        this.HasEveryClaim.ResetToDefault();
    }

    private bool IsClaimsValid(ObservableCollection<string> claims)
        => claims.Count > 0 || this.HasEveryClaim.Input;
}
