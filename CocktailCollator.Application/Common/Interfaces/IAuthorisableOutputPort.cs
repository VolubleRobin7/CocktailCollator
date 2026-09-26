namespace CocktailCollator.Application.Common.Interfaces;

public interface IAuthorisableOutputPort : UseCasePipelines.OutputPorts.IAuthorisableOutputPort
{
    Task Unauthorised(CancellationToken cancellationToken);
}
