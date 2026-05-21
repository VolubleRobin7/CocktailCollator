using AutoMapper;
using CocktailCollator.Web.Common.Generics;
using CocktailCollator.Web.Common.Interfaces;

namespace CocktailCollator.Web.FormModels.Users;

public class ChangePasswordFormModel : IFormModel<ChangePasswordInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<string> NewPassword { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));
    public Guid UserId { get; set; } = Guid.Empty;

    public Action? OnChange { get; set; }

    public ChangePasswordFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.NewPassword.OnChange = () => OnChange?.Invoke();
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
}
