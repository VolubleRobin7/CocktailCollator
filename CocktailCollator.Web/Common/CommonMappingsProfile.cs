using AutoMapper;
using CocktailCollator.Web.Common.Generics;

namespace CocktailCollator.Web.Common;

public class CommonMappingsProfile : Profile
{
    public CommonMappingsProfile()
    {
        _ = this.CreateMap<DocumentInputProperty, IFormFile>().ConstructUsing(input => input.Output!);
    }
}
