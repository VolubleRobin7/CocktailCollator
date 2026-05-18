namespace CocktailCollator.Web.ViewModels.Users;

public class UserViewModel
{
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
