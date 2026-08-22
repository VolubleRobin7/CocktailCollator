using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.Roles;

public class RoleViewModel : IStoreableViewModel<RoleViewModel>
{
    public List<string> Claims { get; set; } = [];
    public bool HasEveryPermissionClaim { get; set; }
    public bool IsDefaultRole { get; set; }
    public required string Name { get; set; }
    public required Guid RoleId { get; set; }

    public void ApplyChanges(RoleViewModel source, IViewModelStore store)
    {
        this.Name = source.Name;
        this.HasEveryPermissionClaim = source.HasEveryPermissionClaim;
        this.IsDefaultRole = source.IsDefaultRole;
        this.Claims = [.. source.Claims];
    }
}
