using CocktailCollator.Infrastructure.Persistence.Models;

namespace CocktailCollator.Web.FormModels.Users;

public class UpdateRolesInputPort
{
    public List<CocktailRole> Roles { get; set; } = [];
    public Guid UserId { get; set; }
}
