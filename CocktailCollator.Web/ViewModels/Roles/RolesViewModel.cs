using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.FormModels.Roles;
using CocktailCollator.Web.Infrastructure.Authentication;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CocktailCollator.Web.ViewModels.Roles;

public class RolesViewModel
{
    private readonly IMapper _mapper;
    private readonly RoleManager<CocktailRole> _roleManager;
    private readonly UserManager<CocktailUser> _userManager;
    private readonly ToastService _toastService;

    public IAsyncRelayCommand<CreateRoleInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }
    public IAsyncRelayCommand<Guid> MarkAsDefaultCommand { get; }
    public IAsyncRelayCommand<UpdateRoleInputPort> UpdateCommand { get; }

    public List<RoleViewModel> Roles { get; private set; } = [];


    public RolesViewModel(
        RoleManager<CocktailRole> roleManager,
        UserManager<CocktailUser> userManager,
        IMapper mapper,
        ToastService toastService)
    {
        this._mapper = mapper;
        this._roleManager = roleManager;
        this._userManager = userManager;
        this._toastService = toastService;

        this.CreateCommand = new AsyncRelayCommand<CreateRoleInputPort>(this.CreateRoleAsync);
        this.DeleteCommand = new AsyncRelayCommand<Guid>(this.DeleteRoleAsync);
        this.GetCommand = new AsyncRelayCommand(this.GetRolesAsync);
        this.MarkAsDefaultCommand = new AsyncRelayCommand<Guid>(this.MarkAsDefaultAsync);
        this.UpdateCommand = new AsyncRelayCommand<UpdateRoleInputPort>(this.UpdateRoleAsync);
    }

    private async Task CreateRoleAsync(CreateRoleInputPort inputPort, CancellationToken cancellationToken)
    {
        try
        {
            if (await this._roleManager.RoleExistsAsync(inputPort.Name))
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Create", $"{inputPort.Name} already exists.");
                return;
            }

            var _Role = new CocktailRole { Name = inputPort.Name, HasEveryPermissionClaim = inputPort.HasEveryPermissionClaim };
            var _Result = await this._roleManager.CreateAsync(_Role);

            if (_Result.Succeeded)
            {
                var _ClaimResults = new List<IdentityResult>();

                var _RoleViewModel = this._mapper.Map<RoleViewModel>(_Role);

                var _ClaimsToAdd = inputPort.HasEveryPermissionClaim ? ClaimValues.Permissions.GetAll() : inputPort.Claims;
                foreach (var _Claim in _ClaimsToAdd)
                {
                    var _ClaimResult = await this._roleManager.AddClaimAsync(_Role, new Claim(Infrastructure.Authentication.ClaimTypes.Permission, _Claim));
                    _ClaimResults.Add(_ClaimResult);
                    if (_ClaimResult.Succeeded)
                        _RoleViewModel.Claims.Add(_Claim);
                }

                this.Roles.Add(_RoleViewModel);

                if (_ClaimResults.Any(r => !r.Succeeded))
                {
                    var _Errors = _ClaimResults.Where(r => !r.Succeeded).SelectMany(r => r.Errors).Select(e => e.Description);
                    this._toastService.ShowToast(ToastType.Warning, "Role Created", $"Role created, but failed to add some claims: {string.Join(", ", _Errors.Distinct())}");
                }
                else
                {
                    this._toastService.ShowToast(ToastType.Success, "Role Created", $"{_Role.Name} created successfully");
                }
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this._toastService.ShowToast(ToastType.Danger, "Failed to Create", $"Failed to create role: {_ErrorMessages}");
            }
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while creating the role: {ex.Message}");
        }
    }

    private async Task UpdateRoleAsync(UpdateRoleInputPort inputPort, CancellationToken cancellationToken)
    {
        try
        {
            var _Role = await this._roleManager.FindByIdAsync(inputPort.RoleId.ToString());
            if (_Role is null)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Update", "Role not found.");
                return;
            }

            IdentityResult _Result = IdentityResult.Success;
            bool _RoleUpdated = false;

            if (_Role.Name != inputPort.Name)
            {
                if (await this._roleManager.RoleExistsAsync(inputPort.Name))
                {
                    this._toastService.ShowToast(ToastType.Danger, "Failed to Update", $"{inputPort.Name} already exists.");
                    return;
                }

                _Role.Name = inputPort.Name;
                _RoleUpdated = true;
            }

            if (_Role.HasEveryPermissionClaim != inputPort.HasEveryPermissionClaim)
            {
                _Role.HasEveryPermissionClaim = inputPort.HasEveryPermissionClaim;
                _RoleUpdated = true;
            }

            if (_RoleUpdated)
            {
                _Result = await this._roleManager.UpdateAsync(_Role);
            }

            if (_Result.Succeeded)
            {
                var _ClaimResults = new List<IdentityResult>();

                var _ExistingRole = this.Roles.FirstOrDefault(r => r.RoleId == _Role.Id);
                if (_ExistingRole != null)
                    _ExistingRole.Name = _Role.Name;

                // Remove all existing permission claims and then add the claims they should have.
                var _CurrentClaims = await this._roleManager.GetClaimsAsync(_Role);
                foreach (var _Claim in _CurrentClaims.Where(c => c.Type == Infrastructure.Authentication.ClaimTypes.Permission))
                {
                    var _ClaimResult = await this._roleManager.RemoveClaimAsync(_Role, _Claim);
                    _ClaimResults.Add(_ClaimResult);
                    if (_ClaimResult.Succeeded && _ExistingRole != null)
                        _ = _ExistingRole.Claims.Remove(_Claim.Value);
                }

                var _ClaimsToAdd = inputPort.HasEveryPermissionClaim ? ClaimValues.Permissions.GetAll() : inputPort.Claims;
                foreach (var _Claim in _ClaimsToAdd)
                {
                    var _ClaimResult = await this._roleManager.AddClaimAsync(_Role, new Claim(Infrastructure.Authentication.ClaimTypes.Permission, _Claim));
                    _ClaimResults.Add(_ClaimResult);
                    if (_ClaimResult.Succeeded && _ExistingRole != null)
                        _ExistingRole.Claims.Add(_Claim);
                }

                if (_ClaimResults.Any(r => !r.Succeeded))
                {
                    var _Errors = _ClaimResults.Where(r => !r.Succeeded).SelectMany(r => r.Errors).Select(e => e.Description);
                    this._toastService.ShowToast(ToastType.Danger, "Failed to Update", $"Failed to update some claims: {string.Join(", ", _Errors.Distinct())}");
                }
                else
                {
                    this._toastService.ShowToast(ToastType.Success, "Role Updated", $"{_Role.Name} updated successfully");
                }
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this._toastService.ShowToast(ToastType.Danger, "Failed to Update", $"Failed to update role: {_ErrorMessages}");
            }
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while updating the role: {ex.Message}");
        }
    }

    private async Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        try
        {
            var _RolesCount = await this._roleManager.Roles.CountAsync(cancellationToken);
            if (_RolesCount <= 1)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Delete", "Cannot delete the last remaining role.");
                return;
            }

            var _Role = await this._roleManager.FindByIdAsync(roleId.ToString());
            if (_Role is null)
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Delete", "Role not found.");
                return;
            }

            var _UsersInRole = await this._userManager.GetUsersInRoleAsync(_Role.Name ?? "");
            if (_UsersInRole.Any())
            {
                this._toastService.ShowToast(ToastType.Danger, "Failed to Delete", "Cannot delete a role that is currently assigned to one or more users.");
                return;
            }

            var _Result = await this._roleManager.DeleteAsync(_Role);

            if (_Result.Succeeded)
            {
                _ = this.Roles.RemoveAll(r => r.RoleId == roleId);
                this._toastService.ShowToast(ToastType.Info, "Role Deleted", $"{_Role.Name} deleted successfully");
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this._toastService.ShowToast(ToastType.Danger, "Failed to Delete", $"Failed to delete role: {_ErrorMessages}");
            }
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while deleting the role: {ex.Message}");
        }
    }

    private async Task MarkAsDefaultAsync(Guid roleId, CancellationToken cancellationToken)
    {
        try
        {
            var _Roles = await this._roleManager.Roles.ToListAsync(cancellationToken);
            foreach (var _Role in _Roles)
            {
                if (_Role.Id == roleId)
                {
                    if (!_Role.DefaultRole)
                    {
                        _Role.DefaultRole = true;
                        var _Result = await this._roleManager.UpdateAsync(_Role);
                        if (_Result.Succeeded)
                        {
                            var _RoleVm = this.Roles.FirstOrDefault(r => r.RoleId == _Role.Id);
                            if (_RoleVm is not null)
                                _RoleVm.IsDefaultRole = true;
                        }
                    }
                }
                else
                {
                    if (_Role.DefaultRole)
                    {
                        _Role.DefaultRole = false;
                        var _Result = await this._roleManager.UpdateAsync(_Role);
                        if (_Result.Succeeded)
                        {
                            var _RoleVm = this.Roles.FirstOrDefault(r => r.RoleId == _Role.Id);
                            if (_RoleVm is not null)
                                _RoleVm.IsDefaultRole = false;
                        }
                    }
                }
            }

            this._toastService.ShowToast(ToastType.Success, "Default Role Updated", $"{_Roles.First(role => role.DefaultRole).Name} is now the default role");
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while setting the default role: {ex.Message}");
        }
    }

    private async Task GetRolesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var _Roles = await this._roleManager.Roles.ToListAsync(cancellationToken);

            var _RoleViewModels = new List<RoleViewModel>();
            foreach (var _Role in _Roles)
            {
                var _RoleViewModel = this._mapper.Map<RoleViewModel>(_Role);
                var _Claims = await this._roleManager.GetClaimsAsync(_Role);
                _RoleViewModel.Claims = [.. _Claims.Select(claim => claim.Value)];
                _RoleViewModels.Add(_RoleViewModel);
            }
            this.Roles = _RoleViewModels;
        }
        catch (Exception ex)
        {
            this._toastService.ShowToast(ToastType.Danger, "Error", $"An error occurred while retrieving roles: {ex.Message}");
        }
    }
}
