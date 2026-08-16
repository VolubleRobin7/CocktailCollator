using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Roles;

public class UpdateRoleFormModel : IFormModel<UpdateRoleInputPort>
{
    private readonly IMapper _mapper;

    public InputPropertyList<string> Claims { get; set; }
    public InputProperty<bool> HasEveryPermissionClaim { get; set; }
        = new(() => false, input => true);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, input => !string.IsNullOrWhiteSpace(input));
    public InputProperty<Guid> RoleId { get; set; }
        = new(() => Guid.Empty, input => input != Guid.Empty);

    public Action? OnChange { get; set; }

    public UpdateRoleFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Claims = new(collectionValidationFunc: (claims) => claims.Any() || this.HasEveryPermissionClaim.Input)
        {
            OnChange = () => OnChange?.Invoke()
        };
        this.HasEveryPermissionClaim.OnChange = () => OnChange?.Invoke();
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
        this.HasEveryPermissionClaim.ResetToDefault();
    }
}
