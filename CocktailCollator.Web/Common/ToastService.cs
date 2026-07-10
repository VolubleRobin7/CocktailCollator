using CocktailCollator.Web.Views.Components.Toasts;

namespace CocktailCollator.Web.Common;

public class ToastService
{
    public event Action<ToastItem>? OnShow;

    /// <summary>
    /// Request to show a toast notification.
    /// </summary>
    /// <param name="type">
    /// The type of the toast notification.
    /// </param>
    /// <param name="title"> 
    /// The title of the toast notification.
    /// Should be worded like an action, e.g. "Recipe Deleted" or "Error Saving Recipe".
    /// </param>
    /// <param name="message">
    /// The message of the toast notification.
    /// </param>
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