using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<UpdateRecipeInputPort, IUpdateRecipeOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageRecipes);
