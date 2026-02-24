using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.GetMeasurements;

public interface IGetMeasurementsOutputPort
{
    Task Success(List<Measurement> measurements, CancellationToken cancellationToken);
}
