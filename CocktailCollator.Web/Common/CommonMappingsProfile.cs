using AutoMapper;
using CocktailCollator.Application.Models;
using CocktailCollator.Web.Common.Generics;

namespace CocktailCollator.Web.Common;

public class CommonMappingsProfile : Profile
{
    public CommonMappingsProfile()
    {
        _ = this.CreateMap<DocumentInputProperty, IFormFile>()
            .ConstructUsing(input => input.Output!);

        _ = this.CreateMap<DocumentInputProperty, DocumentModel>()
            .ForMember(d => d.ExistingDocumentId, o => o.MapFrom(s => s.Existing!.Id))
            .ForMember(d => d.NewDocument, o => o.MapFrom(s => s.Output));

        _ = this.CreateMap<DocumentInputPropertyList, IEnumerable<IFormFile>>()
            .ConstructUsing(inputs => inputs.AsInputs().Where(input => input.Output != null).Select(input => input.Output!).ToList());

        _ = this.CreateMap<DocumentInputPropertyList, IEnumerable<DocumentModel>>()
            .ConstructUsing((inputs, context) => inputs.AsInputs().Select(input => context.Mapper.Map<DocumentModel>(input)).ToList());
    }
}
