using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;
using CocktailCollator.Application.UseCases.Measurements.GetMeasurements;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Measurements;

public class MeasurementsViewModel
{
    public IAsyncRelayCommand<Guid> DeleteCommand { get; set; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<MeasurementViewModel> Measurements { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public MeasurementsViewModel(
        DeleteMeasurementInteractor deleteMeasurementInteractor,
        GetMeasurementsInteractor getMeasurementsInteractor,
        IMapper mapper)
    {
        this.DeleteCommand = new AsyncRelayCommand<Guid>((measurementId, cancellationToken)
            => deleteMeasurementInteractor.Interact(
                new() { MeasurementId = measurementId },
                new DeleteMeasurementPresenter(this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getMeasurementsInteractor.Interact(
                new GetMeasurementsPresenter(mapper, this),
                cancellationToken));
    }

    private class DeleteMeasurementPresenter(MeasurementsViewModel viewModel) : IDeleteMeasurementOutputPort
    {
        Task IDeleteMeasurementOutputPort.Failure(string reason, Measurement? measurement, CancellationToken cancellationToken)
        {
            viewModel.Error = reason;
            return Task.CompletedTask;
        }

        Task IDeleteMeasurementOutputPort.Success(Measurement deletedMeasurement, CancellationToken cancellationToken)
        {
            _ = viewModel.Measurements.RemoveAll(m => m.MeasurementId == deletedMeasurement.MeasurementId);
            return Task.CompletedTask;
        }
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