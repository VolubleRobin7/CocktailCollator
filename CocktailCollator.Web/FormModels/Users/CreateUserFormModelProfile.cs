using AutoMapper;

namespace CocktailCollator.Web.FormModels.Users;

public class CreateUserFormModelProfile : Profile
{
    public CreateUserFormModelProfile()
    {
        _ = this.CreateMap<CreateUserFormModel, CreateUserInputPort>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Username.Input))
            .ForMember(d => d.Password, o => o.MapFrom(s => s.Password.Input));
    }
}
