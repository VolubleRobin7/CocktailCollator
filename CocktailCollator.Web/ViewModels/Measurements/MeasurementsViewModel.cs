using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.GetMeasurements;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Measurements;

public class MeasurementsViewModel
{
    public IAsyncRelayCommand GetCommand { get; }

    public List<MeasurementViewModel> Measurements { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public MeasurementsViewModel(
        GetMeasurementsInteractor getMeasurementsInteractor,
        IMapper mapper)
    {
        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getMeasurementsInteractor.Interact(
                new GetMeasurementsPresenter(mapper, this),
                cancellationToken));
    }

    private class GetMeasurementsPresenter(IMapper mapper, MeasurementsViewModel viewModel) : IGetMeasurementsOutputPort
    {
        Task IGetMeasurementsOutputPort.Success(List<Measurement> measurements, CancellationToken cancellationToken)
        {
            viewModel.Measurements = mapper.Map<List<MeasurementViewModel>>(measurements);
            return Task.CompletedTask;
        }
    }
}