namespace CocktailCollator.Application.Common.Interfaces;

public interface IExistenceOutputPort : UseCasePipelines.OutputPorts.IExistenceOutputPort
{
    Task NotFound(CancellationToken cancellationToken);
}
