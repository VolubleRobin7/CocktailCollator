using AutoMapper;

namespace CocktailCollator.Web.FormModels.Users;

public class UserFormModelProfile : Profile
{
    public UserFormModelProfile()
    {
        _ = this.CreateMap<CreateUserFormModel, CreateUserInputPort>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Input))
            .ForMember(d => d.Password, o => o.MapFrom(s => s.Password.Input))
            .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Input.Select(r => r.RoleId)));

        _ = this.CreateMap<ChangePasswordFormModel, ChangePasswordInputPort>()
            .ForMember(d => d.NewPassword, o => o.MapFrom(s => s.NewPassword.Input));

        _ = this.CreateMap<UpdateRolesFormModel, UpdateRolesInputPort>()
            .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles.Input.Select(r => r.RoleId)));
    }
}
