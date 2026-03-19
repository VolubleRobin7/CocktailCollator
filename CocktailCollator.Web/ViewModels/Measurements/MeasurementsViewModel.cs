using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;
using CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;
using CocktailCollator.Application.UseCases.Measurements.GetMeasurements;
using CocktailCollator.Domain.Entities;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Measurements;

public class MeasurementsViewModel
{
    public IAsyncRelayCommand<CreateMeasurementInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<MeasurementViewModel> Measurements { get; private set; } = [];

    public string Error { get; private set; } = string.Empty;

    public MeasurementsViewModel(
        CreateMeasurementInteractor createMeasurementInteractor,
        DeleteMeasurementInteractor deleteMeasurementInteractor,
        GetMeasurementsInteractor getMeasurementsInteractor,
        IMapper mapper)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateMeasurementInputPort>((inputPort, cancellationToken)
            => createMeasurementInteractor.Interact(
                inputPort,
                new CreateMeasurementPresenter(mapper, this),
                cancellationToken));
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

    private class CreateMeasurementPresenter(IMapper mapper, MeasurementsViewModel viewModel) : ICreateMeasurementOutputPort
    {
        Task ICreateMeasurementOutputPort.Success(Measurement measurement, CancellationToken cancellationToken)
        {
            viewModel.Measurements.Add(mapper.Map<MeasurementViewModel>(measurement));
            return Task.CompletedTask;
        }
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