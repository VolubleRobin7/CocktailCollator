using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.Documents;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        _ = this.CreateMap<Document, DocumentViewModel>()
            .ForMember(d => d.Url, o => o.MapFrom(s => "/files/" + s.FilePath));
    }
}
