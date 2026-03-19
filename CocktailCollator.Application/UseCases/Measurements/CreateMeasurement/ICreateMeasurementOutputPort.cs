using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public interface ICreateMeasurementOutputPort
{
    Task Success(Measurement measurement, CancellationToken cancellationToken);
}
