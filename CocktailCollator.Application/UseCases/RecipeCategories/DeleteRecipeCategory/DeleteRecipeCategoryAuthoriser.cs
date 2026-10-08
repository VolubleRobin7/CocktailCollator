using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.RecipeCategories.DeleteRecipeCategory;

public class DeleteRecipeCategoryAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<DeleteRecipeCategoryInputPort, IDeleteRecipeCategoryOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageRecipes);
