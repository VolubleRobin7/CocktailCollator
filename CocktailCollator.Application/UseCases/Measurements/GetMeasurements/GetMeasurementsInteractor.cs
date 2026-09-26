using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.UseCases.Measurements.GetMeasurements;

public class GetMeasurementsInteractor(ICocktailDbContext dbContext) : IInteractorPipe<GetMeasurementsInputPort, IGetMeasurementsOutputPort>
{
    public Task ExecuteAsync(GetMeasurementsInputPort inputPort, IGetMeasurementsOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<Measurement>()], cancellationToken);
}
