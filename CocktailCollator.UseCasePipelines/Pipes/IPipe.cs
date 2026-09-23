using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.UseCasePipelines.Pipes;

public interface IPipe<in TInputPort, in TOutputPort> where TInputPort : IInputPort<TOutputPort>
{
    Task<bool> ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken);
}
