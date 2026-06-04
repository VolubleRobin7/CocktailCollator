namespace CocktailCollator.Web.FormModels.Users;

public class UpdateRolesInputPort
{
    public bool IsAdmin { get; set; }
    public bool IsUser { get; set; }
    public Guid UserId { get; set; }
}
