using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

namespace CocktailCollator.Web.FormModels.Measurements;

public class MeasurementFormModelProfile : Profile
{
    public MeasurementFormModelProfile()
    {
        _ = this.CreateMap<CreateMeasurementFormModel, CreateMeasurementInputPort>();
    }
}
