using AutoMapper;

namespace CocktailCollator.Web.FormModels.Users;

public class UserFormModelProfile : Profile
{
    public UserFormModelProfile()
    {
        _ = this.CreateMap<CreateUserFormModel, CreateUserInputPort>()
            .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(r => r.RoleId)));

        _ = this.CreateMap<ChangePasswordFormModel, ChangePasswordInputPort>();

        _ = this.CreateMap<UpdateRolesFormModel, UpdateRolesInputPort>()
            .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Select(r => r.RoleId)));
    }
}
