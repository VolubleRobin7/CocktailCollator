using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;

public class CreateMeasurementAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<CreateMeasurementInputPort, ICreateMeasurementOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageMeasurements);
