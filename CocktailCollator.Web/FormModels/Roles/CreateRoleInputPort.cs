namespace CocktailCollator.Web.FormModels.Roles;

public class CreateRoleInputPort
{
    public List<string> Claims { get; set; } = [];
    public bool HasEveryPermissionClaim { get; set; }
    public string Name { get; set; } = string.Empty;
}
