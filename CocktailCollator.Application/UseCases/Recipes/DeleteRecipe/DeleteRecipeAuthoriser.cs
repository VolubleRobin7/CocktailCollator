using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using CocktailCollator.UseCasePipelines.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Recipes.DeleteRecipe;

public class DeleteRecipeAuthoriser(
    IPipeline<DeleteRecipeInputPort, IDeleteRecipeOutputPort> innerPipe,
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<DeleteRecipeInputPort, IDeleteRecipeOutputPort>(
        innerPipe,
        authenticationStateProvider,
        authorisationService,
        Policies.ManageRecipes);
