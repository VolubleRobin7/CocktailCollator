using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Measurements;

public class CreateMeasurementFormModel(IMapper mapper) : IFormModel<CreateMeasurementInputPort>
{
    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public CreateMeasurementInputPort ExtractToInputPort()
        => mapper.Map<CreateMeasurementInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid();

    public void ResetToDefault()
        => this.Name.ResetToDefault();
}
