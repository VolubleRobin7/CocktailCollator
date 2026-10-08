using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public interface ICreateMeasurementOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Success(Measurement measurement, CancellationToken cancellationToken);
}
