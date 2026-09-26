using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public class CreateMeasurementInputPort : IInputPort<ICreateMeasurementOutputPort>
{
    public required string Name { get; set; }
}
