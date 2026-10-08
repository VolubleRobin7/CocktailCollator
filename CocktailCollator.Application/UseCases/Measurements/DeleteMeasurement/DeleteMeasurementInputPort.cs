using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;

public class DeleteMeasurementInputPort : IInputPort<IDeleteMeasurementOutputPort>
{
    public required Guid MeasurementId { get; set; }
}
