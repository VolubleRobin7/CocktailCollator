using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Infrastructure.Persistence.Models;

public class CocktailRole : IdentityRole<Guid>
{
    public bool DefaultRole { get; set; }
    public bool HasEveryPermissionClaim { get; set; }
}
