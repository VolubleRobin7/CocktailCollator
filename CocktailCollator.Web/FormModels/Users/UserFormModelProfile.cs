using AutoMapper;

namespace CocktailCollator.Web.FormModels.Users;

public class UserFormModelProfile : Profile
{
    public UserFormModelProfile()
    {
        _ = this.CreateMap<CreateUserFormModel, CreateUserInputPort>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Input))
            .ForMember(d => d.Password, o => o.MapFrom(s => s.Password.Input));

        _ = this.CreateMap<ChangePasswordFormModel, ChangePasswordInputPort>()
            .ForMember(d => d.NewPassword, o => o.MapFrom(s => s.NewPassword.Input));

        _ = this.CreateMap<UpdateRolesFormModel, UpdateRolesInputPort>()
            .ForMember(d => d.IsAdmin, o => o.MapFrom(s => s.IsAdmin.Input))
            .ForMember(d => d.IsUser, o => o.MapFrom(s => s.IsUser.Input));
    }
}
