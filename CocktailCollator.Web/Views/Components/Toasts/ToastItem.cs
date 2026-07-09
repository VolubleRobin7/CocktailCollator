namespace CocktailCollator.Web.Views.Components.Toasts;

public class ToastItem
{
    public string Id { get; } = $"toast_{Guid.NewGuid()}";
    public string Message { get; init; } = string.Empty;
    public DateTime Time { get; } = DateTime.Now;
    public string Title { get; init; } = string.Empty;
    public ToastType Type { get; init; } = ToastType.Info;
}

public enum ToastType
{
    Info,
    Success,
    Error
}