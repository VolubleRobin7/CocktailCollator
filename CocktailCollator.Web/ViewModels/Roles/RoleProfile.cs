using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;

namespace CocktailCollator.Web.ViewModels.Roles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        _ = this.CreateMap<CocktailRole, RoleViewModel>()
            .ForMember(d => d.RoleId, o => o.MapFrom(s => s.Id));
    }
}
