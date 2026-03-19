using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public class CreateMeasurementInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(CreateMeasurementInputPort inputPort, ICreateMeasurementOutputPort outputPort, CancellationToken cancellationToken)
    {
        Measurement _Measurement = new() { Name = inputPort.Name };
        dbContext.Add(_Measurement);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Measurement, cancellationToken);
    }
}
