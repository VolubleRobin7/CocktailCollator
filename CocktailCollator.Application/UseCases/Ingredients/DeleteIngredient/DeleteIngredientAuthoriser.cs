using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Ingredients.DeleteIngredient;

public class DeleteIngredientAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<DeleteIngredientInputPort, IDeleteIngredientOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageIngredients);
