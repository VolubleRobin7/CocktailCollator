using CocktailCollator.UseCasePipelines.InputPorts;
using CocktailCollator.UseCasePipelines.OutputPorts;

namespace CocktailCollator.UseCasePipelines.Pipes;

public interface IAuthorisationPipe<in TInputPort, in TOutputPort> : IPipe<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
    where TOutputPort : IAuthorisableOutputPort
{
}
