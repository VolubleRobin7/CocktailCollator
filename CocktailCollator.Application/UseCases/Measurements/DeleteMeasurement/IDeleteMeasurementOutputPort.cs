using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;

public interface IDeleteMeasurementOutputPort
{
    Task Failure(string failureReason, Measurement? measurement, CancellationToken cancellationToken);

    Task Success(Measurement measurement, CancellationToken cancellationToken);
}
