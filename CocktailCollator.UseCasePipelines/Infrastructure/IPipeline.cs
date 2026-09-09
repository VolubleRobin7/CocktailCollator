namespace CocktailCollator.UseCasePipelines.Infrastructure;

public interface IPipeline<TInputPort, TOutputPort> where TInputPort : IInputPort<TOutputPort>
{
    Task ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken);
}
