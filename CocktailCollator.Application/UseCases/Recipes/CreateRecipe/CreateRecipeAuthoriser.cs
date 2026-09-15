using CocktailCollator.Application.Common.Pipes;
using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.CreateRecipe;

public class CreateRecipeAuthoriser(
    IPipeline<CreateRecipeInputPort, ICreateRecipeOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<CreateRecipeInputPort, ICreateRecipeOutputPort>(
        innerPipe,
        authenticationStateProvider,
        authorisationService,
        "ManageRecipes");
