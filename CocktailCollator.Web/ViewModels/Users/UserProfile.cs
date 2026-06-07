using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;

namespace CocktailCollator.Web.ViewModels.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        _ = this.CreateMap<CocktailUser, UserViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}
