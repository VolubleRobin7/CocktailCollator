using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.UseCasePipelines.InputPorts;
using CocktailCollator.UseCasePipelines.Pipes;

namespace CocktailCollator.Application.Common.Pipes;

public abstract class ExistencePipeBase<TInputPort, TOutputPort, TDomainEntity>(
    ICocktailDbContext dbContext,
    Func<TInputPort, Guid> inputPortIdSelector)
    : IExistencePipe<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
    where TOutputPort : IExistenceOutputPort
    where TDomainEntity : class
{
    public virtual Task<bool> ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
        => ExistencePipeBaseHelper.CheckExistenceAsync<TDomainEntity>(dbContext, inputPortIdSelector.Invoke(inputPort), outputPort, cancellationToken);
}

// There is no 1-generic-parameter interface implementation (output port only) as existence checks always require an input port to provide the entity ID.

file static class ExistencePipeBaseHelper
{
    public static async Task<bool> CheckExistenceAsync<TDomainEntity>(
        ICocktailDbContext dbContext,
        Guid id,
        IExistenceOutputPort outputPort,
        CancellationToken cancellationToken)
        where TDomainEntity : class
    {
        var _EntityExists = await dbContext.ExistsAsync<TDomainEntity>(id, cancellationToken);
        if (_EntityExists)
            return true;
        else
        {
            await outputPort.NotFound(cancellationToken);
            return false;
        }
    }
}
