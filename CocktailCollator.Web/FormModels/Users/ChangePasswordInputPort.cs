namespace CocktailCollator.Web.FormModels.Users;

public class ChangePasswordInputPort
{
    public string NewPassword { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
