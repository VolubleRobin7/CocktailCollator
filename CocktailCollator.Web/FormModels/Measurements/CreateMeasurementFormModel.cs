using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;
using CocktailCollator.Web.Common.Inputs;

namespace CocktailCollator.Web.FormModels.Measurements;

public class CreateMeasurementFormModel : IFormModel<CreateMeasurementInputPort>
{
    private readonly IMapper _mapper;

    public InputProperty<string> Name { get; set; }
        = new(() => string.Empty, (input) => !string.IsNullOrEmpty(input));

    public Action? OnChange { get; set; }

    public CreateMeasurementFormModel(IMapper mapper)
    {
        this._mapper = mapper;

        this.Name.OnChange = () => OnChange?.Invoke();
    }

    public CreateMeasurementInputPort ExtractToInputPort()
        => this._mapper.Map<CreateMeasurementInputPort>(this);

    public bool IsValid()
        => this.Name.IsValid();

    public void ResetToDefault()
        => this.Name.ResetToDefault();
}
