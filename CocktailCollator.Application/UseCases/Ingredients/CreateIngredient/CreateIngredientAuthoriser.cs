using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Ingredients.CreateIngredient;

public class CreateIngredientAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<CreateIngredientInputPort, ICreateIngredientOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageIngredients);
