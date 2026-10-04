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

// TODO: Is it even possible for an existence check to not have an input port? If not, remove this class and just use the above one.
public abstract class ExistencePipeBase<TOutputPort, TDomainEntity>(
    ICocktailDbContext dbContext,
    Guid id)
    : IExistencePipe<EmptyInputPort<TOutputPort>, TOutputPort>
    where TOutputPort : IExistenceOutputPort
    where TDomainEntity : class
{
    public virtual Task<bool> ExecuteAsync(TOutputPort outputPort, CancellationToken cancellationToken)
        => ExistencePipeBaseHelper.CheckExistenceAsync<TDomainEntity>(dbContext, id, outputPort, cancellationToken);

    // Explicit interface implementation to satisfy the pipeline engine's 2-generic-parameter contract.
    // Consumers will use ExecuteAsync() above.
    Task<bool> IPipe<EmptyInputPort<TOutputPort>, TOutputPort>.ExecuteAsync(
        EmptyInputPort<TOutputPort> inputPort,
        TOutputPort outputPort,
        CancellationToken cancellationToken)
        => this.ExecuteAsync(outputPort, cancellationToken);
}

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
