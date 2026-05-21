using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Users;

public class CreateUserFormModel : IFormModel<CreateUserInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<string> Password { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public InputProperty<string> Username { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateUserFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Password.OnChange = () => OnChange?.Invoke();
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
}
