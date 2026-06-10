namespace CocktailCollator.Web.FormModels.Users;

public class UpdateRolesInputPort
{
    public List<Guid> Roles { get; set; } = [];
    public Guid UserId { get; set; }
}
