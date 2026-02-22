using AutoMapper;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Web.ViewModels.Measurements;

public class MeasurementProfile : Profile
{
    public MeasurementProfile()
    {
        _ = this.CreateMap<Measurement, MeasurementViewModel>();
    }
}
