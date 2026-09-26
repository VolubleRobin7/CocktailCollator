using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Ingredients.GetIngredients;

public class GetIngredientsAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<IGetIngredientsOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ViewIngredients);
