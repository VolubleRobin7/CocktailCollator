using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Measurements.GetMeasurements;

public class GetMeasurementsAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<IGetMeasurementsOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ViewMeasurements);
