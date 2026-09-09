namespace CocktailCollator.UseCasePipelines.Infrastructure;

public interface IInteractorPipe<TInputPort, TOutputPort> : IPipeline<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
{ }
