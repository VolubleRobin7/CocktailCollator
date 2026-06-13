namespace CocktailCollator.Web.FormModels.Roles;

public class UpdateRoleInputPort
{
    public List<string> Claims { get; set; } = [];
    public bool HasEveryClaim { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}
