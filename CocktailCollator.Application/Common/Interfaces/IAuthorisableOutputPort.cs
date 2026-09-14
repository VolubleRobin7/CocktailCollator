namespace CocktailCollator.Application.Common.Interfaces;

public interface IAuthorisableOutputPort
{
    Task Unauthorised(CancellationToken cancellationToken);
}
