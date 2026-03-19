using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public class CreateMeasurementInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(CreateMeasurementInputPort inputPort, ICreateMeasurementOutputPort outputPort, CancellationToken cancellationToken)
    {
        Measurement _Measurement = new() { Name = inputPort.Name };
        dbContext.Add(_Measurement);
        return outputPort.Success(_Measurement, cancellationToken);
    }
}
