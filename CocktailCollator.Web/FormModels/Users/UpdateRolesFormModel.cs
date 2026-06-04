using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Users;

public class UpdateRolesFormModel : IFormModel<UpdateRolesInputPort>
{
    private readonly IMapper _mapper;

    public Guid UserId { get; set; } = Guid.Empty;
    public InputProperty<bool> IsAdmin { get; set; }
        = new(() => false, (_) => true);
    public InputProperty<bool> IsUser { get; set; }
        = new(() => false, (_) => true);

    public Action? OnChange { get; set; }

    public UpdateRolesFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.IsAdmin.OnChange = () => OnChange?.Invoke();
        this.IsUser.OnChange = () => OnChange?.Invoke();
    }

    public UpdateRolesInputPort ExtractToInputPort()
        => this._mapper.Map<UpdateRolesInputPort>(this);

    public bool IsValid() => true;

    public void ResetToDefault()
    {
        this.UserId = Guid.Empty;
        this.IsAdmin.ResetToDefault();
        this.IsUser.ResetToDefault();
    }
}
