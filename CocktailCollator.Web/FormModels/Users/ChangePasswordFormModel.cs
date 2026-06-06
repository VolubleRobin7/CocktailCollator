using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CocktailCollator.Web.FormModels.Users;

public class ChangePasswordFormModel : IFormModel<ChangePasswordInputPort>
{
    private readonly IMapper _mapper;
    private readonly PasswordOptions _passwordOptions;

    public InputProperty<string> NewPassword { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;

    public Action? OnChange { get; set; }

    public ChangePasswordFormModel(IOptions<IdentityOptions> identityOptions, IMapper mapper)
    {
        this._mapper = mapper;
        this._passwordOptions = identityOptions.Value.Password;

        this.NewPassword = new(() => string.Empty, this.CheckPasswordPolicy)
        {
            OnChange = () => OnChange?.Invoke()
        };
    }

    public ChangePasswordInputPort ExtractToInputPort()
        => this._mapper.Map<ChangePasswordInputPort>(this);

    public bool IsValid()
        => this.NewPassword.IsValid();

    public void ResetToDefault()
    {
        this.UserId = Guid.Empty;
        this.NewPassword.ResetToDefault();
    }

    private bool CheckPasswordPolicy(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        if (input.Length < this._passwordOptions.RequiredLength)
            return false;

        if (this._passwordOptions.RequireDigit && !input.Any(char.IsDigit))
            return false;

        if (this._passwordOptions.RequireUppercase && !input.Any(char.IsUpper))
            return false;

        if (this._passwordOptions.RequireLowercase && !input.Any(char.IsLower))
            return false;

        if (this._passwordOptions.RequireNonAlphanumeric && !input.Any(c => !char.IsLetterOrDigit(c)))
            return false;

        return true;
    }
}
