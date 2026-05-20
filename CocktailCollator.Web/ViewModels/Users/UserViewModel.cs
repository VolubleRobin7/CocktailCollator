namespace CocktailCollator.Web.ViewModels.Users;

public class UserViewModel
{
    public string? Email { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
}
