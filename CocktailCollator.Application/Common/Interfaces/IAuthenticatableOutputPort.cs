namespace CocktailCollator.Application.Common.Interfaces;

public interface IAuthenticatableOutputPort : UseCasePipelines.OutputPorts.IAuthenticatableOutputPort
{
    Task Unauthenticated(CancellationToken cancellationToken);
}