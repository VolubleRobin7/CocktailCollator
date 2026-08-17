using AutoMapper;

namespace CocktailCollator.Web.FormModels.Roles;

public class RoleFormModelProfile : Profile
{
    public RoleFormModelProfile()
    {
        _ = this.CreateMap<CreateRoleFormModel, CreateRoleInputPort>();

        _ = this.CreateMap<UpdateRoleFormModel, UpdateRoleInputPort>();
    }
}
