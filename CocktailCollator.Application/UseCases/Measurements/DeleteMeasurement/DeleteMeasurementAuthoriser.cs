using CocktailCollator.Application.Common.Authorisation;
using CocktailCollator.Application.Common.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;

public class DeleteMeasurementAuthoriser(
    AuthenticationStateProvider authenticationStateProvider,
    IAuthorizationService authorisationService)
    : AuthorisationPipeBase<DeleteMeasurementInputPort, IDeleteMeasurementOutputPort>(
        authenticationStateProvider,
        authorisationService,
        Policies.ManageMeasurements);
