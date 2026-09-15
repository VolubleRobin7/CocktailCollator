using CocktailCollator.Application.Common.Pipes;
using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.UpdateRecipe;

public class UpdateRecipeAuthoriser(
    IPipeline<UpdateRecipeInputPort, IUpdateRecipeOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<UpdateRecipeInputPort, IUpdateRecipeOutputPort>(
        innerPipe,
        authenticationStateProvider,
        authorisationService,
        "ManageRecipes");
