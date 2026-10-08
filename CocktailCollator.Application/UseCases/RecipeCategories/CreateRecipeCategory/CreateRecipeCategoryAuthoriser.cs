using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.RecipeCategories.CreateRecipeCategory;

public class CreateRecipeCategoryAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<CreateRecipeCategoryInputPort, ICreateRecipeCategoryOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageRecipes);
