using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CocktailCollator.Web.FormModels.Users;

public class CreateUserFormModel : IFormModel<CreateUserInputPort>
{
    private readonly IMapper _mapper;
    private readonly PasswordOptions _passwordOptions;

    public InputProperty<string> Password { get; set; }
    public InputProperty<string> Username { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateUserFormModel(IOptions<IdentityOptions> identityOptions, IMapper mapper)
    {
        this._mapper = mapper;
        this._passwordOptions = identityOptions.Value.Password;

        this.Password = new(() => string.Empty, this.CheckPasswordPolicy)
        {
            OnChange = () => OnChange?.Invoke()
        };
        this.Username.OnChange = () => OnChange?.Invoke();
    }

    public CreateUserInputPort ExtractToInputPort()
        => this._mapper.Map<CreateUserInputPort>(this);

    public bool IsValid()
        => this.Username.IsValid() && this.Password.IsValid();

    public void ResetToDefault()
    {
        this.Username.ResetToDefault();
        this.Password.ResetToDefault();
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
