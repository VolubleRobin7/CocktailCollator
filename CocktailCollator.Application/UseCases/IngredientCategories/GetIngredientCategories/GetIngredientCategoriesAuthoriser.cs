using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.IngredientCategories.GetIngredientCategories;

public class GetIngredientCategoriesAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<GetIngredientCategoriesInputPort, IGetIngredientCategoriesOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ViewIngredients);
