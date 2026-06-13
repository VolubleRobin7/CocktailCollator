namespace CocktailCollator.Web.ViewModels.Roles;

public class RoleViewModel
{
    public List<string> Claims { get; set; } = [];
    public required string Name { get; set; }
    public required Guid RoleId { get; set; }
}
