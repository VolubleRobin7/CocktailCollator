using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using System.Collections.ObjectModel;

namespace CocktailCollator.Web.FormModels.Users;

public class UpdateRolesFormModel : IFormModel<UpdateRolesInputPort>
{
    private readonly IMapper _mapper;

    public Guid UserId { get; set; } = Guid.Empty;
    public InputProperty<ObservableCollection<UpdateRolesFormModelRole>> Roles { get; set; }
        = new(() => [], (_) => true);

    public Action? OnChange { get; set; }

    public UpdateRolesFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Roles.OnChange = () => OnChange?.Invoke();
        this.Roles.Input.CollectionChanged += (_, _) => OnChange?.Invoke();
    }

    public UpdateRolesInputPort ExtractToInputPort()
        => this._mapper.Map<UpdateRolesInputPort>(this);

    public bool IsValid() => true;

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
