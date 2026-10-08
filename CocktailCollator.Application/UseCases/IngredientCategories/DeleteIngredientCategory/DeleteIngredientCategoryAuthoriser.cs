using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.IngredientCategories.DeleteIngredientCategory;

public class DeleteIngredientCategoryAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<DeleteIngredientCategoryInputPort, IDeleteIngredientCategoryOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageIngredients);
