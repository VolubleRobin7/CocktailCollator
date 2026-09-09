using CocktailCollator.Application.Common.Interfaces;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Views.Components.Toasts;

namespace CocktailCollator.Web.Common.Presenters;

public abstract class BasePresenter(ToastService toastService, string actionDescription = "perform this action") : IAuthenticatableOutputPort
{
    public virtual Task Unauthenticated(CancellationToken cancellationToken)
    {
        toastService.ShowToast(ToastType.Warning, "Access Denied", $"You must be logged in to {actionDescription}.");
        return Task.CompletedTask;
    }
}