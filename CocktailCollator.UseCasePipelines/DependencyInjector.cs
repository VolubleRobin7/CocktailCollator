using CocktailCollator.UseCasePipelines.InputPorts;
using CocktailCollator.UseCasePipelines.OutputPorts;
using CocktailCollator.UseCasePipelines.Pipes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CocktailCollator.UseCasePipelines;

public static class DependencyInjector
{
    private sealed record InteractorRegistration(Type InteractorType, Type InputType, Type OutputType);
    public sealed record PipelineStage(
        Type OutputPortInterface,
        Type PipeInterface,
        Type? DefaultPipeImplementation = null);

    // Default execution order of pipeline stages (from first to last).
    private static readonly PipelineStage[] s_defaultPipelineStageDefinitions =
    [
        new(typeof(IAuthenticatableOutputPort), typeof(IAuthenticationPipe<,>)),
        new(typeof(IAuthorisableOutputPort), typeof(IAuthorisationPipe<,>)),
        new(typeof(IExistenceOutputPort), typeof(IExistencePipe<,>))
    ];

    /// <summary>
    /// Discovers all the use case pipelines in the provided assemblies, registering each use case 
    /// as <see cref="IPipeline{TInputPort, TOutputPort}"/>.
    /// </summary>
    /// <remarks>
    /// Pipeline pipes are included based on the interfaces implemented by each use case's output port.
    /// </remarks>
    public static IServiceCollection AddUseCasePipelines(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddUseCasePipelines(s_defaultPipelineStageDefinitions, assemblies);

    /// <summary>
    /// Discovers all the use case pipelines in the provided assemblies, registering each use case 
    /// as <see cref="IPipeline{TInputPort, TOutputPort}"/> with the configured pipeline order.
    /// </summary>
    /// <remarks>
    /// Pipeline pipes are included based on the interfaces implemented by each use case's output port.
    /// </remarks>
    /// <param name="pipelineStages">
    /// The pipeline order to apply on top of the interactor, in execution order.
    /// <para>Example: <c>[new(typeof(IAuthenticatableOutputPort), typeof(IAuthenticationPipe&lt;,&gt;)), new(typeof(IExistenceOutputPort), typeof(IExistencePipe&lt;,&gt;))]</c></para>
    /// </param>
    public static IServiceCollection AddUseCasePipelines(
        this IServiceCollection services,
        PipelineStage[] pipelineStages,
        params Assembly[] assemblies)
    {
        ValidatePipelineStages(pipelineStages);

        var _AssemblyList = assemblies.ToList();
        if (_AssemblyList.Count == 0)
            throw new ArgumentException("At least one assembly must be provided to scan for use cases.", nameof(assemblies));

        var _CandidateTypes = _AssemblyList
            .SelectMany(a => a.GetExportedTypes())
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .ToList();

        // 1. Find all classes implementing IInteractorPipe<TInputPort, TOutputPort>
        var _InteractorRegistrations = new List<InteractorRegistration>();

        foreach (var _CandidateType in _CandidateTypes.Where(t => !t.IsGenericTypeDefinition))
        {
            var _InteractorInterfaces = _CandidateType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInteractorPipe<,>))
                .ToList();

            foreach (var _InteractorInterface in _InteractorInterfaces)
            {
                var _InputType = _InteractorInterface.GenericTypeArguments[0];
                var _OutputType = _InteractorInterface.GenericTypeArguments[1];
                _InteractorRegistrations.Add(new InteractorRegistration(_CandidateType, _InputType, _OutputType));
            }
        }

        // Validate no duplicate interactors for the same use case
        var _DuplicateInteractors = _InteractorRegistrations
            .GroupBy(r => (r.InputType, r.OutputType))
            .FirstOrDefault(g => g.Count() > 1);

        if (_DuplicateInteractors != null)
        {
            var (_InType, _OutType) = _DuplicateInteractors.Key;
            throw new InvalidOperationException(
                $"Multiple interactors found for use case {_InType.Name} -> {_OutType.Name}: " +
                $"{string.Join(", ", _DuplicateInteractors.Select(r => r.InteractorType.FullName))}");
        }

        // 2. Build the pipeline for each use case
        foreach (var _Interactor in _InteractorRegistrations)
        {
            // Register concrete interactor type in DI
            _ = services.AddScoped(_Interactor.InteractorType);

            // Find matching pipes according to the defined pipeline stages.
            var _ActivePipeTypes = new List<Type>();

            foreach (var _StageDef in pipelineStages)
            {
                // Only include this stage if TOutputPort implements the stage's marker interface
                if (!_StageDef.OutputPortInterface.IsAssignableFrom(_Interactor.OutputType))
                    continue;

                var _ClosedPipeInterface = _StageDef.PipeInterface.MakeGenericType(_Interactor.InputType, _Interactor.OutputType);
                var _MatchingPipeTypes = new List<Type>();

                // Attempt to find a matching specific pipe implementation for this stage
                foreach (var _CandidateType in _CandidateTypes)
                {
                    if (!_CandidateType.IsGenericTypeDefinition && _ClosedPipeInterface.IsAssignableFrom(_CandidateType))
                        _MatchingPipeTypes.Add(_CandidateType);
                }

                // Attempt to use the default pipe implementation if no matching pipes were found
                if (_MatchingPipeTypes.Count == 0 && _StageDef.DefaultPipeImplementation != null)
                {
                    if (_StageDef.DefaultPipeImplementation.IsGenericTypeDefinition)
                    {
                        try
                        {
                            var _ClosedDefault = _StageDef.DefaultPipeImplementation.MakeGenericType(_Interactor.InputType, _Interactor.OutputType);
                            if (_ClosedPipeInterface.IsAssignableFrom(_ClosedDefault))
                                _MatchingPipeTypes.Add(_ClosedDefault);
                        }
                        catch (ArgumentException)
                        {
                            // Type constraints not satisfied
                        }
                    }
                    else if (_ClosedPipeInterface.IsAssignableFrom(_StageDef.DefaultPipeImplementation))
                        _MatchingPipeTypes.Add(_StageDef.DefaultPipeImplementation);
                }

                if (_MatchingPipeTypes.Count > 1)
                {
                    throw new InvalidOperationException(
                        $"Multiple implementations of {_ClosedPipeInterface.Name} found for use case {_Interactor.InputType.Name} -> {_Interactor.OutputType.Name}: " +
                        $"{string.Join(", ", _MatchingPipeTypes.Select(t => t.FullName))}");
                }

                if (_MatchingPipeTypes.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Output port '{_Interactor.OutputType.Name}' implements '{_StageDef.OutputPortInterface.Name}', " +
                        $"but no matching pipe implementing '{_ClosedPipeInterface.Name}' was found for use case {_Interactor.InputType.Name} -> {_Interactor.OutputType.Name}.");
                }

                var _PipeType = _MatchingPipeTypes[0];
                _ = services.AddScoped(_PipeType);
                _ActivePipeTypes.Add(_PipeType);
            }

            // 3. Register IPipeline<TInputPort, TOutputPort>
            var _RegisterMethod = typeof(DependencyInjector)
                .GetMethod(nameof(RegisterPipeline), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(_Interactor.InputType, _Interactor.OutputType);

            _ = _RegisterMethod.Invoke(null, [services, _Interactor.InteractorType, _ActivePipeTypes.ToArray()]);
        }

        return services;
    }

    private static void RegisterPipeline<TInputPort, TOutputPort>(
        IServiceCollection services,
        Type interactorType,
        Type[] pipeTypes)
        where TInputPort : IInputPort<TOutputPort>
    {
        _ = services.AddScoped<IPipeline<TInputPort, TOutputPort>>(sp =>
        {
            var _Interactor = (IInteractorPipe<TInputPort, TOutputPort>)sp.GetRequiredService(interactorType);
            var _Pipes = new IPipe<TInputPort, TOutputPort>[pipeTypes.Length];
            for (var i = 0; i < pipeTypes.Length; i++)
                _Pipes[i] = (IPipe<TInputPort, TOutputPort>)sp.GetRequiredService(pipeTypes[i]);

            return new UseCasePipeline<TInputPort, TOutputPort>(_Pipes, _Interactor);
        });
    }

    private static void ValidatePipelineStages(PipelineStage[] pipelineStages)
    {
        foreach (var _StageDef in pipelineStages)
        {
            if (_StageDef.DefaultPipeImplementation == null)
                continue;

            var _DefaultType = _StageDef.DefaultPipeImplementation;
            var _PipeInterface = _StageDef.PipeInterface;

            if (!_DefaultType.IsClass || _DefaultType.IsAbstract)
            {
                throw new InvalidOperationException(
                    $"Default pipe implementation '{_DefaultType.FullName}' for stage '{_StageDef.OutputPortInterface.Name}' must be a non-abstract class.");
            }

            if (_PipeInterface.IsGenericTypeDefinition)
            {
                if (!_DefaultType.IsGenericTypeDefinition || _DefaultType.GetGenericArguments().Length != _PipeInterface.GetGenericArguments().Length)
                {
                    throw new InvalidOperationException(
                        $"Default pipe implementation '{_DefaultType.FullName}' must be an open-generic type definition with {_PipeInterface.GetGenericArguments().Length} type argument(s) to match '{_PipeInterface.FullName}'.");
                }

                var _ImplementsGeneric = _DefaultType.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == _PipeInterface);

                if (!_ImplementsGeneric)
                {
                    throw new InvalidOperationException(
                        $"Default pipe implementation '{_DefaultType.FullName}' does not implement the required pipe interface '{_PipeInterface.FullName}' for stage '{_StageDef.OutputPortInterface.Name}'.");
                }
            }
            else
            {
                if (!_PipeInterface.IsAssignableFrom(_DefaultType))
                {
                    throw new InvalidOperationException(
                        $"Default pipe implementation '{_DefaultType.FullName}' does not implement the required pipe interface '{_PipeInterface.FullName}' for stage '{_StageDef.OutputPortInterface.Name}'.");
                }
            }
        }
    }
}
