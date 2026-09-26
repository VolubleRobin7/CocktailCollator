using CocktailCollator.UseCasePipelines.InputPorts;

namespace CocktailCollator.UseCasePipelines.Pipes;

public class UseCasePipeline<TInputPort, TOutputPort>(IReadOnlyList<IPipe<TInputPort, TOutputPort>> pipes, IInteractorPipe<TInputPort, TOutputPort> interactor)
    : IPipeline<TInputPort, TOutputPort> where TInputPort : IInputPort<TOutputPort>
{
    public async Task ExecuteAsync(TInputPort inputPort, TOutputPort outputPort, CancellationToken cancellationToken)
    {
        for (var i = 0; i < pipes.Count; i++)
        {
            var _ShouldContinue = await pipes[i].ExecuteAsync(inputPort, outputPort, cancellationToken);
            if (!_ShouldContinue)
                return;
        }

        await interactor.ExecuteAsync(inputPort, outputPort, cancellationToken);
    }
}

public class ParameterlessUseCasePipeline<TOutputPort>(
    IReadOnlyList<IPipe<EmptyInputPort<TOutputPort>, TOutputPort>> pipes,
    IInteractorPipe<EmptyInputPort<TOutputPort>, TOutputPort> interactor)
    : IPipeline<TOutputPort>
{
    private static readonly EmptyInputPort<TOutputPort> s_empty = new();

    public async Task ExecuteAsync(TOutputPort outputPort, CancellationToken cancellationToken)
    {
        for (var i = 0; i < pipes.Count; i++)
        {
            var _ShouldContinue = await pipes[i].ExecuteAsync(s_empty, outputPort, cancellationToken);
            if (!_ShouldContinue)
                return;
        }

        await interactor.ExecuteAsync(s_empty, outputPort, cancellationToken);
    }
}
