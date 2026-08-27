using AutoMapper;
using CocktailCollator.Web.Common.Inputs;

namespace CocktailCollator.Web.FormModels.Roles;

public class CreateRoleFormModel : IFormModel<CreateRoleInputPort>
{
    private readonly IMapper _mapper;

    public InputPropertyList<string> Claims { get; set; }
    public InputProperty<bool> HasEveryPermissionClaim { get; set; }
        = new(() => false, input => true);
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, input => !string.IsNullOrWhiteSpace(input));

    public Action? OnChange { get; set; }

    public CreateRoleFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Claims = new(collectionValidationFunc: (claims) => claims.Any() || this.HasEveryPermissionClaim.Input)
        {
            OnChange = () => OnChange?.Invoke()
        };
        this.HasEveryPermissionClaim.OnChange = () => OnChange?.Invoke();
        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public CreateRoleInputPort ExtractToInputPort()
        => this._mapper.Map<CreateRoleInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid() && this.Claims.IsValid();

    public void ResetToDefault()
    {
        this.Name.ResetToDefault();
        this.Claims.ResetToDefault();
        this.HasEveryPermissionClaim.ResetToDefault();
    }
}
