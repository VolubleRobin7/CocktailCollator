namespace CocktailCollator.UseCasePipelines.Infrastructure;

public interface IAuthorisationPipe<TInputPort, TOutputPort> : IPipeline<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
{
    IPipeline<TInputPort, TOutputPort> InnerPipe { get; }
}
