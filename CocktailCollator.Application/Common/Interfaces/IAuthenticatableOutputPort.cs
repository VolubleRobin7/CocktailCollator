namespace CocktailCollator.Application.Common.Interfaces;

public interface IAuthenticatableOutputPort
{
    Task Unauthenticated(CancellationToken cancellationToken);
}