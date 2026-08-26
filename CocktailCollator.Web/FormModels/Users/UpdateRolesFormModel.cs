using AutoMapper;
using CocktailCollator.Web.Common.Inputs;

namespace CocktailCollator.Web.FormModels.Users;

public class UpdateRolesFormModel : IFormModel<UpdateRolesInputPort>
{
    private readonly IMapper _mapper;

    public InputPropertyList<UpdateRolesFormModelRole> Roles { get; set; }
        = new(collectionValidationFunc: (roles) => roles.Any());
    public Guid UserId { get; set; } = Guid.Empty;

    public Action? OnChange { get; set; }

    public UpdateRolesFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Roles.OnChange = () => OnChange?.Invoke();
    }

    public UpdateRolesInputPort ExtractToInputPort()
        => this._mapper.Map<UpdateRolesInputPort>(this);

    public bool IsValid()
        => this.Roles.IsValid();

    public void ResetToDefault()
    {
        this.UserId = Guid.Empty;
        this.Roles.ResetToDefault();
    }
}

public class UpdateRolesFormModelRole
{
    public Guid RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
}
