using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.GetMeasurements;

public class GetMeasurementsInteractor(ICocktailDbContext dbContext)
{
    public Task Interact(IGetMeasurementsOutputPort outputPort, CancellationToken cancellationToken)
        => outputPort.Success([.. dbContext.GetEntities<Measurement>()], cancellationToken);
}
