using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web.FormModels.Roles;
using CocktailCollator.Web.Infrastructure.Authentication;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CocktailCollator.Web.ViewModels.Roles;

public class RolesViewModel
{
    private readonly IMapper _mapper;
    private readonly RoleManager<CocktailRole> _roleManager;

    public IAsyncRelayCommand<CreateRoleInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }
    public IAsyncRelayCommand<UpdateRoleInputPort> UpdateCommand { get; }

    public List<RoleViewModel> Roles { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public RolesViewModel(
        RoleManager<CocktailRole> roleManager,
        IMapper mapper)
    {
        this._mapper = mapper;
        this._roleManager = roleManager;

        this.CreateCommand = new AsyncRelayCommand<CreateRoleInputPort>(this.CreateRoleAsync);
        this.DeleteCommand = new AsyncRelayCommand<Guid>(this.DeleteRoleAsync);
        this.GetCommand = new AsyncRelayCommand(this.GetRolesAsync);
        this.UpdateCommand = new AsyncRelayCommand<UpdateRoleInputPort>(this.UpdateRoleAsync);
    }

    private async Task CreateRoleAsync(CreateRoleInputPort inputPort, CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            if (await this._roleManager.RoleExistsAsync(inputPort.Name))
            {
                this.Error = "A role with that name already exists.";
                return;
            }

            var _Role = new CocktailRole { Name = inputPort.Name };
            var _Result = await this._roleManager.CreateAsync(_Role);

            if (_Result.Succeeded)
            {
                var _ClaimsToAdd = inputPort.HasEveryClaim ? ClaimValues.Permissions.GetAll() : inputPort.Claims;
                foreach (var _Claim in _ClaimsToAdd)
                    _ = await this._roleManager.AddClaimAsync(_Role, new Claim(Infrastructure.Authentication.ClaimTypes.Permission, _Claim));

                await this.GetRolesAsync(cancellationToken);
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this.Error = $"Failed to create role: {_ErrorMessages}";
            }
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while creating the role: {ex.Message}";
        }
    }

    private async Task UpdateRoleAsync(UpdateRoleInputPort inputPort, CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            var _Role = await this._roleManager.FindByIdAsync(inputPort.RoleId.ToString());
            if (_Role is null)
            {
                this.Error = "Role not found.";
                return;
            }

            if (_Role.Name != inputPort.Name && await this._roleManager.RoleExistsAsync(inputPort.Name))
            {
                this.Error = "A role with that name already exists.";
                return;
            }

            _Role.Name = inputPort.Name;
            var _Result = await this._roleManager.UpdateAsync(_Role);

            if (_Result.Succeeded)
            {
                // Remove all existing claims and then add the claims they should have.
                var _CurrentClaims = await this._roleManager.GetClaimsAsync(_Role);
                var _PermissionClaims = _CurrentClaims.Where(c => c.Type == Infrastructure.Authentication.ClaimTypes.Permission);
                foreach (var _Claim in _PermissionClaims)
                    _ = await this._roleManager.RemoveClaimAsync(_Role, _Claim);

                var _ClaimsToAdd = inputPort.HasEveryClaim ? ClaimValues.Permissions.GetAll() : inputPort.Claims;
                foreach (var _Claim in _ClaimsToAdd)
                    _ = await this._roleManager.AddClaimAsync(_Role, new Claim(Infrastructure.Authentication.ClaimTypes.Permission, _Claim));

                await this.GetRolesAsync(cancellationToken);
            }
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this.Error = $"Failed to update role: {_ErrorMessages}";
            }
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while updating the role: {ex.Message}";
        }
    }

    private async Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            var _RolesCount = await this._roleManager.Roles.CountAsync(cancellationToken);
            if (_RolesCount <= 1)
            {
                this.Error = "Cannot delete the last remaining role.";
                return;
            }

            var _Role = await this._roleManager.FindByIdAsync(roleId.ToString());
            if (_Role is null)
            {
                this.Error = "Role not found.";
                return;
            }

            var _Result = await this._roleManager.DeleteAsync(_Role);

            if (_Result.Succeeded)
                _ = this.Roles.RemoveAll(r => r.RoleId == roleId);
            else
            {
                var _ErrorMessages = string.Join(", ", _Result.Errors.Select(e => e.Description));
                this.Error = $"Failed to delete role: {_ErrorMessages}";
            }
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while deleting the role: {ex.Message}";
        }
    }

    private async Task GetRolesAsync(CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;
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
            this.Error = $"An error occurred while retrieving roles: {ex.Message}";
        }
    }
}
