using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Users;

public class CreateUserFormModel : IFormModel<CreateUserInputPort>
{
    private readonly bool _enforcePasswordPolicies;
    private readonly IMapper _mapper;

    public InputProperty<string> Password { get; set; }
    public InputProperty<string> Username { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateUserFormModel(IConfiguration configuration, IMapper mapper)
    {
        this._mapper = mapper;
        this._enforcePasswordPolicies = configuration.GetValue<bool>("EnforcePasswordPolicies");

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

        if (this._enforcePasswordPolicies)
        {
            if (input.Length < 8)
                return false;

            if (!input.Any(char.IsDigit))
                return false;

            if (!input.Any(char.IsUpper))
                return false;

            if (!input.Any(c => !char.IsLetterOrDigit(c)))
                return false;
        }

        return true;
    }
}
