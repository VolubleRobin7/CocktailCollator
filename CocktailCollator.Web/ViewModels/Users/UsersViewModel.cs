using AutoMapper;
using CocktailCollator.Infrastructure.Persistence.Models;
using CocktailCollator.Web.FormModels.Users;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CocktailCollator.Web.ViewModels.Users;

public class UsersViewModel
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IMapper _mapper;
    private readonly UserManager<CocktailUser> _userManager;

    public IAsyncRelayCommand<CreateUserInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }
    public IAsyncRelayCommand<ChangePasswordInputPort> ChangePasswordCommand { get; }

    public UserViewModel? CurrentUser { get; private set; }
    public List<UserViewModel> Users { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public UsersViewModel(
        UserManager<CocktailUser> userManager,
        AuthenticationStateProvider authenticationStateProvider,
        IMapper mapper)
    {
        this._authenticationStateProvider = authenticationStateProvider;
        this._mapper = mapper;
        this._userManager = userManager;

        this.CreateCommand = new AsyncRelayCommand<CreateUserInputPort>((inputPort, cancellationToken)
            => this.CreateUserAsync(inputPort.Username, inputPort.Password, cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>(this.DeleteUserAsync);

        this.GetCommand = new AsyncRelayCommand(this.GetUsersAsync);

        this.ChangePasswordCommand = new AsyncRelayCommand<ChangePasswordInputPort>((inputPort, cancellationToken)
            => this.ChangePasswordAsync(inputPort.UserId, inputPort.NewPassword, cancellationToken));
    }

    private async Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            var _User = await this._userManager.FindByIdAsync(userId.ToString());

            if (_User is null)
            {
                this.Error = "User not found.";
                return;
            }

            var _Token = await this._userManager.GeneratePasswordResetTokenAsync(_User);
            var _Result = await this._userManager.ResetPasswordAsync(_User, _Token, newPassword);

            if (!_Result.Succeeded)
            {
                var _ErrorMessages = string.Join(" ", _Result.Errors.Select(e => e.Description));
                this.Error = $"Failed to change password: {_ErrorMessages}";
            }
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while changing the password: {ex.Message}";
        }
    }

    private async Task CreateUserAsync(string username, string password, CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            var _User = new CocktailUser
            {
                UserName = username,
            };

            var _Result = await this._userManager.CreateAsync(_User, password);

            if (_Result.Succeeded)
                this.Users.Add(this._mapper.Map<UserViewModel>(_User));
            else
            {
                var _ErrorMessages = string.Join(" ", _Result.Errors.Select(e => e.Description));
                this.Error = $"Failed to create user: {_ErrorMessages}";
            }
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while creating the user: {ex.Message}";
        }
    }

    private async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            var _User = await this._userManager.FindByIdAsync(userId.ToString());

            if (_User is null)
            {
                this.Error = "User not found.";
                return;
            }

            var _Result = await this._userManager.DeleteAsync(_User);

            if (_Result.Succeeded)
                _ = this.Users.RemoveAll(u => u.UserId == userId);
            else
            {
                var _ErrorMessages = string.Join(" ", _Result.Errors.Select(e => e.Description));
                this.Error = $"Failed to delete user: {_ErrorMessages}";
            }
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while deleting the user: {ex.Message}";
        }
    }

    private async Task GetUsersAsync(CancellationToken cancellationToken)
    {
        try
        {
            this.Error = string.Empty;

            var _AuthState = await this._authenticationStateProvider.GetAuthenticationStateAsync();
            this.CurrentUser = this._mapper.Map<UserViewModel>(await this._userManager.GetUserAsync(_AuthState.User));
            this.Users = this._mapper.Map<List<UserViewModel>>(this._userManager.Users);
        }
        catch (Exception ex)
        {
            this.Error = $"An error occurred while retrieving users: {ex.Message}";
        }
    }
}
