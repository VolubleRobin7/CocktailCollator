using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Domain.Entities;

namespace CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;

public interface IDeleteMeasurementOutputPort : IAuthenticatableOutputPort, IAuthorisableOutputPort
{
    Task Failure(string failureReason, Measurement? measurement, CancellationToken cancellationToken);

    Task Success(Measurement measurement, CancellationToken cancellationToken);
}
