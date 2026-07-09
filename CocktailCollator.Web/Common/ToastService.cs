using CocktailCollator.Web.Views.Components.Toasts;

namespace CocktailCollator.Web.Common;

public class ToastService
{
    public event Action<ToastItem>? OnShow;

    public void ShowToast(ToastType type, string title, string message)
    {
        var _ToastItem = new ToastItem
        {
            Title = title,
            Message = message,
            Type = type
        };
        this.OnShow?.Invoke(_ToastItem);
    }
}