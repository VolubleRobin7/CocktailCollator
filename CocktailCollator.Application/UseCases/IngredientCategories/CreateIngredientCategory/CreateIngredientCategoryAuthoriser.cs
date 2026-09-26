using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.IngredientCategories.CreateIngredientCategory;

public class CreateIngredientCategoryAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<CreateIngredientCategoryInputPort, ICreateIngredientCategoryOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageIngredients);
