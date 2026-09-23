using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Views.Components.Toasts;

namespace CocktailCollator.Web.Common.Presenters;

public abstract class BasePresenter(ToastService toastService, string actionDescription = "perform this action")
    : IAuthenticatableOutputPort, IAuthorisableOutputPort, IExistenceOutputPort
{
    public virtual Task Unauthenticated(CancellationToken cancellationToken)
    {
        toastService.ShowToast(ToastType.Danger, "Access Denied", $"You must be logged in to {actionDescription}.");
        return Task.CompletedTask;
    }

    public virtual Task Unauthorised(CancellationToken cancellationToken)
    {
        toastService.ShowToast(ToastType.Danger, "Permission Denied", $"You do not have permission to {actionDescription}.");
        return Task.CompletedTask;
    }

    public virtual Task NotFound(CancellationToken cancellationToken)
    {
        toastService.ShowToast(ToastType.Warning, "Not Found", $"The item you are trying to {actionDescription} for does not exist.");
        return Task.CompletedTask;
    }
}