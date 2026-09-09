namespace CocktailCollator.UseCasePipelines.Infrastructure;

public interface IAuthenticationPipe<TInputPort, TOutputPort> : IPipeline<TInputPort, TOutputPort>
    where TInputPort : IInputPort<TOutputPort>
{
    IPipeline<TInputPort, TOutputPort> InnerPipe { get; }
}