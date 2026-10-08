using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.GetMeasurements;

public interface IGetMeasurementsOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(List<Measurement> measurements, CancellationToken cancellationToken);
}
