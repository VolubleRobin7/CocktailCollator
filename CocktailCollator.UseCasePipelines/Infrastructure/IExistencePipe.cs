namespace CocktailCollator.UseCasePipelines.Infrastructure;

public interface IExistencePipe<TInputPort, TOutputPort> : IPipeline<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
{
    IPipeline<TInputPort, TOutputPort> InnerPipe { get; }
}
