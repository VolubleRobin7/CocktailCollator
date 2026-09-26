using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public class CreateMeasurementInteractor(ICocktailDbContext dbContext) : IInteractorPipe<CreateMeasurementInputPort, ICreateMeasurementOutputPort>
{
    public async Task ExecuteAsync(CreateMeasurementInputPort inputPort, ICreateMeasurementOutputPort outputPort, CancellationToken cancellationToken)
    {
        Measurement _Measurement = new() { Name = inputPort.Name };
        dbContext.Add(_Measurement);
        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Measurement, cancellationToken);
    }
}
