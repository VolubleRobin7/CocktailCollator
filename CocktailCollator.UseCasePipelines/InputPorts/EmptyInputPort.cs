namespace CocktailCollator.UseCasePipelines.InputPorts;

/// <summary>
/// A synthetic placeholder input port used internally by the pipeline engine for parameterless use cases. Consumers can write clean, 
/// single-parameter interactors (<c>IInteractorPipe&lt;TOutputPort&gt;</c>) and view models can inject single-parameter 
/// pipelines (<c>IPipeline&lt;TOutputPort&gt;</c>).
/// </summary>
/// <remarks>
/// Consumer projects should <b>not</b> instantiate this type directly or use it as a regular input model in use cases or view models.
/// In consumer projects, this type should only be referenced when implementing intermediate pipe base classes that must satisfy 
/// the engine's two-parameter <c>IPipe&lt;EmptyInputPort&lt;TOutputPort&gt;, TOutputPort&gt;</c> contract via explicit interface 
/// implementation while exposing a clean parameterless method to derived classes.
/// </remarks>
/// <example>
/// How to implement a custom authorisation pipe base class for parameterless use cases:
/// <code>
/// public abstract class AuthorisationPipeBase&lt;TOutputPort&gt;(...)
///     : IAuthorisationPipe&lt;EmptyInputPort&lt;TOutputPort&gt;, TOutputPort&gt;
///     where TOutputPort : IAuthorisableOutputPort
/// {
///     // Clean parameterless method exposed to consumers/derived classes:
///     public virtual Task&lt;bool&gt; ExecuteAsync(TOutputPort outputPort, CancellationToken cancellationToken)
///     {
///         // Authorization logic...
///     }
///
///     // Explicit interface implementation satisfying the pipeline engine's 2-parameter contract:
///     Task&lt;bool&gt; IPipe&lt;EmptyInputPort&lt;TOutputPort&gt;, TOutputPort&gt;.ExecuteAsync(
///         EmptyInputPort&lt;TOutputPort&gt; inputPort,
///         TOutputPort outputPort,
///         CancellationToken cancellationToken)
///         =&gt; ExecuteAsync(outputPort, cancellationToken);
/// }
/// </code>
/// </example>
public sealed record EmptyInputPort<TOutputPort> : IInputPort<TOutputPort>;
