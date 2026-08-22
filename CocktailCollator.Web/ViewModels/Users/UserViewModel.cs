using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.ViewModels.Roles;

namespace CocktailCollator.Web.ViewModels.Users;

public class UserViewModel : IStoreableViewModel<UserViewModel>
{
    public string? Email { get; set; }
    public required Guid UserId { get; set; }
    public string? UserName { get; set; }
    public List<RoleViewModel> Roles { get; set; } = [];

    public void ApplyChanges(UserViewModel source, IViewModelStore store)
    {
        this.Email = source.Email;
        this.UserName = source.UserName;

        if (source.Roles is not null)
            this.Roles = [.. source.Roles.Select(r => store.UpdateOrRegister(r.RoleId, r))];
    }
}
