using AutoMapper;

namespace CocktailCollator.Web.FormModels.Roles;

public class RoleFormModelProfile : Profile
{
    public RoleFormModelProfile()
    {
        _ = this.CreateMap<CreateRoleFormModel, CreateRoleInputPort>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.Claims, o => o.MapFrom(s => s.Claims.Input))
            .ForMember(d => d.HasEveryPermissionClaim, o => o.MapFrom(s => s.HasEveryPermissionClaim.Input));

        _ = this.CreateMap<UpdateRoleFormModel, UpdateRoleInputPort>()
            .ForMember(d => d.RoleId, o => o.MapFrom(s => s.RoleId.Input))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Input))
            .ForMember(d => d.Claims, o => o.MapFrom(s => s.Claims.Input))
            .ForMember(d => d.HasEveryPermissionClaim, o => o.MapFrom(s => s.HasEveryPermissionClaim.Input));
    }
}
