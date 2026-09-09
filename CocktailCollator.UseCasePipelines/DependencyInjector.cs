using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CocktailCollator.UseCasePipelines;

public static class DependencyInjector
{
    private sealed record InteractorRegistration(Type InteractorType, Type InputType, Type OutputType);

    // Default execution order of pipeline stages (from outermost to innermost decorator).
    private static readonly Type[] s_defaultPipelineStageDefinitions =
    [
        typeof(IAuthenticationPipe<,>),
        typeof(IExistencePipe<,>)
    ];

    /// <summary>
    /// Discovers all the use case pipelines in the provided assemblies, registering each use case 
    /// as <see cref="IPipeline{TInputPort, TOutputPort}"/>.
    /// </summary>
    public static IServiceCollection AddUseCasePipelines(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddUseCasePipelines(s_defaultPipelineStageDefinitions, [.. assemblies]);

    /// <summary>
    /// Discovers all the use case pipelines in the provided assemblies, registering each use case 
    /// as <see cref="IPipeline{TInputPort, TOutputPort}"/> with the configured decorator pipeline.
    /// </summary>
    /// <param name="pipelineStages">
    /// The decorator pipeline stages to apply on top of the interactor, in execution order (outermost to innermost).
    /// <para>Example: <c>[typeof(IAuthenticationPipe&lt;,&gt;), typeof(IExistencePipe&lt;,&gt;)]</c></para>
    /// </param>
    public static IServiceCollection AddUseCasePipelines(
        this IServiceCollection services,
        Type[] pipelineStages,
        params Assembly[] assemblies)
    {
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
            var _PipelineInterfaceType = typeof(IPipeline<,>).MakeGenericType(_Interactor.InputType, _Interactor.OutputType);

            // Register concrete interactor type in DI
            _ = services.AddScoped(_Interactor.InteractorType);

            // Find matching decorators according to the defined pipeline stages.
            // pipelineStages is in execution order (outermost to innermost).
            // To wrap the core interactor, we wrap in reverse order (innermost decorator first, outermost decorator last).
            var _DecoratorsInWrappingOrder = new List<Type>();

            foreach (var _StageDef in pipelineStages.Reverse())
            {
                var _ClosedStageInterface = _StageDef.MakeGenericType(_Interactor.InputType, _Interactor.OutputType);
                var _MatchingDecorators = new List<Type>();

                foreach (var _CandidateType in _CandidateTypes)
                {
                    if (_CandidateType.IsGenericTypeDefinition)
                    {
                        if (_CandidateType.GetGenericArguments().Length == 2)
                        {
                            try
                            {
                                var _ClosedCandidate = _CandidateType.MakeGenericType(_Interactor.InputType, _Interactor.OutputType);
                                if (_ClosedStageInterface.IsAssignableFrom(_ClosedCandidate))
                                    _MatchingDecorators.Add(_ClosedCandidate);
                            }
                            catch (ArgumentException)
                            {
                                // Type constraints not met for this use case
                            }
                        }
                    }
                    else if (_ClosedStageInterface.IsAssignableFrom(_CandidateType))
                        _MatchingDecorators.Add(_CandidateType);
                }

                if (_MatchingDecorators.Count > 1)
                {
                    throw new InvalidOperationException(
                        $"Multiple implementations of {_ClosedStageInterface.Name} found for use case {_Interactor.InputType.Name} -> {_Interactor.OutputType.Name}: " +
                        $"{string.Join(", ", _MatchingDecorators.Select(t => t.FullName))}");
                }

                if (_MatchingDecorators.Count == 1)
                {
                    var _DecoratorType = _MatchingDecorators[0];
                    _DecoratorsInWrappingOrder.Add(_DecoratorType);
                    // Register concrete decorator type in DI
                    _ = services.AddScoped(_DecoratorType);
                }
            }

            // Precompile factories at startup using ActivatorUtilities for zero runtime reflection overhead
            var _CoreFactory = ActivatorUtilities.CreateFactory(_Interactor.InteractorType, Type.EmptyTypes);
            var _StageFactories = _DecoratorsInWrappingOrder
                .Select(decType => ActivatorUtilities.CreateFactory(decType, [_PipelineInterfaceType]))
                .ToList();

            // Register IPipeline<TInputPort, TOutputPort> factory
            _ = services.AddScoped(_PipelineInterfaceType, sp =>
            {
                var _CurrentPipe = _CoreFactory(sp, null);

                foreach (var _Factory in _StageFactories)
                {
                    _CurrentPipe = _Factory(sp, [_CurrentPipe]);
                }

                return _CurrentPipe;
            });
        }

        return services;
    }
}
