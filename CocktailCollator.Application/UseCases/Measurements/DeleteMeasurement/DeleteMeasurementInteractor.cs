using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;

public class DeleteMeasurementInteractor(ICocktailDbContext dbContext)
{
    public async Task Interact(DeleteMeasurementInputPort inputPort, IDeleteMeasurementOutputPort outputPort, CancellationToken cancellationToken)
    {
        var _Measurement = dbContext.GetEntities<Measurement>().First(measurement => measurement.MeasurementId == inputPort.MeasurementId);

        if (dbContext.GetEntities<Ingredient>().Any(i => i.Measurements!.Any(im => im.MeasurementId == inputPort.MeasurementId)))
        {
            await outputPort.Failure("Ingredients are still using this measurement.", _Measurement, cancellationToken);
            return;
        }

        dbContext.Remove(_Measurement);

        await dbContext.SaveChangesAsync(cancellationToken);
        await outputPort.Success(_Measurement, cancellationToken);
    }
}
