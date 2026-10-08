using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.UseCasePipelines.Pipes;

public interface IInteractorPipe<in TInputPort, in TOutputPort> where TInputPort : IInputPort<TOutputPort>
{
    Task ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken);
}

public interface IInteractorPipe<in TOutputPort>
{
    Task ExecuteAsync(TOutputPort outputPort, CancellationToken cancellationToken);
}
