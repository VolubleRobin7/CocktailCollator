using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<CreateRecipeInputPort, ICreateRecipeOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageRecipes);
