using CocktailCollator.UseCasePipelines.InputPorts;
using CocktailCollator.UseCasePipelines.OutputPorts;

namespace CocktailCollator.UseCasePipelines.Pipes;

public interface IExistencePipe<in TInputPort, in TOutputPort> : IPipe<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
    where TOutputPort : IExistenceOutputPort
{
}
