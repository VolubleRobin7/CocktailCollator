using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.FormModels.Users;
using CocktailCollator.Web.ViewModels.Roles;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Web.ViewModels.Users;

public class UsersViewModel
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IMapper _mapper;
    private readonly RolesViewModel _rolesViewModel;
    private readonly UserManager<CocktailUser> _userManager;
    private readonly IViewModelStore _store;
    private readonly ToastService _toastService;

    public IAsyncRelayCommand<CreateUserInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }
    public IAsyncRelayCommand<ChangePasswordInputPort> ChangePasswordCommand { get; }
    public IAsyncRelayCommand<UpdateRolesInputPort> UpdateRolesCommand { get; }

    public UserViewModel? CurrentUser { get; private set; }
    public List<UserViewModel> Users { get; private set; } = [];

    public UsersViewModel(
        UserManager<CocktailUser> userManager,
        AuthenticationStateProvider authenticationStateProvider,
        IMapper mapper,
        RolesViewModel rolesViewModel,
        IViewModelStore store,
        ToastService toastService)
    {
        this._authenticationStateProvider = authenticationStateProvider;
        this._mapper = mapper;
        this._userManager = userManager;
        this._rolesViewModel = rolesViewModel;
        this._store = store;
        this._toastService = toastService;

        this.CreateCommand = new AsyncRelayCommand<CreateUserInputPort>(this.CreateUserAsync);

        this.DeleteCommand = new AsyncRelayCommand<Guid>(this.DeleteUserAsync);

        this.GetCommand = new AsyncRelayCommand(this.GetUsersAsync);

        this.ChangePasswordCommand = new AsyncRelayCommand<ChangePasswordInputPort>((inputPort, cancellationToken)
            => this.ChangePasswordAsync(inputPort.UserId, inputPort.NewPassword, cancellationToken));

        this.UpdateRolesCommand = new AsyncRelayCommand<UpdateRolesInputPort>(this.UpdateRolesAsync);
    }

    private async Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken)
    {
        try
        {
            var _User = await this._userManager.FindByIdAsync(userId.ToString());

            if (_User is null)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Change Password", "User not found.");
                return;
            }

            var _Token = await this._userManager.GeneratePasswordResetTokenAsync(_User);
            var _Result = await this._userManager.ResetPasswordAsync(_User, _Token, newPassword);

            if (!_Result.Succeeded)
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this._toastService.ShowToast(ToastType.Danger, "Failed to Change Password", $"Failed to change password: {_ErrorMessages}");
            }
            else
            {
                this._toastService.ShowToast(ToastType.Success, "Password Changed", "Password has been successfully changed.");
            }
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while changing the password: {ex.Message}");
        }
    }

    private async Task CreateUserAsync(CreateUserInputPort inputPort, CancellationToken cancellationToken)
    {
        try
        {
            if (inputPort.Roles.Count == 0)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Create User", "A user must have at least one role assigned.");
                return;
            }

            var _User = new CocktailUser { UserName = inputPort.Username, };

            var _Result = await this._userManager.CreateAsync(_User, inputPort.Password);

            if (_Result.Succeeded)
            {
                await this._rolesViewModel.GetCommand.ExecuteAsync(null);
                var _TargetRoleNames = this._rolesViewModel.Roles
                    .Where(r => inputPort.Roles.Contains(r.RoleId))
                    .Select(r => r.Name);

                var _RoleResult = await this._userManager.AddToRolesAsync(_User, _TargetRoleNames);
                if (!_RoleResult.Succeeded)
                {
                    _ = await this._userManager.DeleteAsync(_User);
                    var _RoleErrors = string.Join(", ", _RoleResult.Errors.Select(e => e.Description));
                    this._toastService.ShowToast(ToastType.Danger, "Failed to Create User", $"Failed to assign roles to the new user: {_RoleErrors}");
                    return;
                }

                var _UserViewModel = this._mapper.Map<UserViewModel>(_User);
                _UserViewModel.Roles = [.. this._rolesViewModel.Roles.Where(r => _TargetRoleNames.Contains(r.Name))];
                this.Users.Add(this._store.UpdateOrRegister(_UserViewModel.UserId, _UserViewModel));
                this._toastService.ShowToast(ToastType.Success, "User Created", $"{_User.UserName} created successfully");
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this._toastService.ShowToast(ToastType.Danger, "Failed to Create User", $"Failed to create user: {_ErrorMessages}");
            }
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while creating the user: {ex.Message}");
        }
    }

    private async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var _User = await this._userManager.FindByIdAsync(userId.ToString());

            if (_User is null)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Delete User", "User not found.");
                return;
            }

            var _Result = await this._userManager.DeleteAsync(_User);

            if (_Result.Succeeded)
            {
                _ = this.Users.RemoveAll(u => u.UserId == userId);
                this._store.Remove<UserViewModel>(userId);
                this._toastService.ShowToast(ToastType.Info, "User Deleted", $"{_User.UserName} deleted successfully");
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this._toastService.ShowToast(ToastType.Danger, "Failed to Delete User", $"Failed to delete user: {_ErrorMessages}");
            }
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while deleting the user: {ex.Message}");
        }
    }

    private async Task GetUsersAsync(CancellationToken cancellationToken)
    {
        try
        {
            await this._rolesViewModel.GetCommand.ExecuteAsync(null);

            var _Users = new List<UserViewModel>();
            foreach (var _DomainUser in this._userManager.Users.ToList())
            {
                var _User = this._mapper.Map<UserViewModel>(_DomainUser);
                var roleNames = await this._userManager.GetRolesAsync(_DomainUser);
                _User.Roles = [.. this._rolesViewModel.Roles.Where(r => roleNames.Contains(r.Name))];
                _Users.Add(this._store.UpdateOrRegister(_User.UserId, _User));
            }

            this.Users = _Users;

            var _AuthState = await this._authenticationStateProvider.GetAuthenticationStateAsync();
            var _DomainCurrentUser = await this._userManager.GetUserAsync(_AuthState.User);
            this.CurrentUser = _DomainCurrentUser is not null
                ? this._store.Get<UserViewModel>(_DomainCurrentUser.Id)
                : null;
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while retrieving users: {ex.Message}");
        }
    }

    private async Task UpdateRolesAsync(UpdateRolesInputPort inputPort, CancellationToken cancellationToken)
    {
        try
        {
            var _User = await this._userManager.FindByIdAsync(inputPort.UserId.ToString());
            if (_User is null)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Update Roles", "User not found.");
                return;
            }

            if (inputPort.Roles.Count == 0)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Update Roles", "A user must have at least one role assigned.");
                return;
            }

            var _CurrentRoleNames = await this._userManager.GetRolesAsync(_User);
            await this._rolesViewModel.GetCommand.ExecuteAsync(null);
            var _TargetRoleNames = this._rolesViewModel.Roles
                .Where(r => inputPort.Roles.Contains(r.RoleId))
                .Select(r => r.Name)
                .ToList();

            var _RolesToAdd = _TargetRoleNames.Except(_CurrentRoleNames).ToList();
            var _RolesToRemove = _CurrentRoleNames.Except(_TargetRoleNames).ToList();

            if (_RolesToAdd.Count > 0)
                _ = await this._userManager.AddToRolesAsync(_User, _RolesToAdd);

            if (_RolesToRemove.Count > 0)
                _ = await this._userManager.RemoveFromRolesAsync(_User, _RolesToRemove);

            _ = await this._userManager.UpdateSecurityStampAsync(_User);

            var _UpdatedUser = this._mapper.Map<UserViewModel>(_User);
            _UpdatedUser.Roles = [.. this._rolesViewModel.Roles.Where(r => _TargetRoleNames.Contains(r.Name))];
            _ = this._store.UpdateOrRegister(_UpdatedUser.UserId, _UpdatedUser);

            this._toastService.ShowToast(ToastType.Success, "User Updated", $"Roles for {_User.UserName} updated successfully");
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while updating roles: {ex.Message}");
        }
    }
}
