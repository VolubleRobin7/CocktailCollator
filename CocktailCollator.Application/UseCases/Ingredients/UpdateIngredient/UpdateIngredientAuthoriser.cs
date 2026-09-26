using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Ingredients.UpdateIngredient;

public class UpdateIngredientAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<UpdateIngredientInputPort, IUpdateIngredientOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageIngredients);
