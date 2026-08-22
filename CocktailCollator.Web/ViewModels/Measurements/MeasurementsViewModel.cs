using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;
using CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;
using CocktailCollator.Application.UseCases.Measurements.GetMeasurements;
using CocktailCollator.Domain.Entities;
using CocktailCollator.Web.Common.Services;
using CocktailCollator.Web.Common.State;
using CocktailCollator.Web.Views.Components.Toasts;
using CommunityToolkit.Mvvm.Input;

namespace CocktailCollator.Web.ViewModels.Measurements;

public class MeasurementsViewModel
{
    public IAsyncRelayCommand<CreateMeasurementInputPort> CreateCommand { get; }
    public IAsyncRelayCommand<Guid> DeleteCommand { get; }
    public IAsyncRelayCommand GetCommand { get; }

    public List<MeasurementViewModel> Measurements { get; private set; } = [];


    public MeasurementsViewModel(
        CreateMeasurementInteractor createMeasurementInteractor,
        DeleteMeasurementInteractor deleteMeasurementInteractor,
        GetMeasurementsInteractor getMeasurementsInteractor,
        IMapper mapper,
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateMeasurementInputPort>((inputPort, cancellationToken)
            => createMeasurementInteractor.Interact(
                inputPort,
                new CreateMeasurementPresenter(mapper, toastService, store, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((measurementId, cancellationToken)
            => deleteMeasurementInteractor.Interact(
                new() { MeasurementId = measurementId },
                new DeleteMeasurementPresenter(toastService, store, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getMeasurementsInteractor.Interact(
                new GetMeasurementsPresenter(mapper, store, this),
                cancellationToken));
    }

    private class CreateMeasurementPresenter(IMapper mapper, ToastService toastService, IViewModelStore store, MeasurementsViewModel viewModel) : ICreateMeasurementOutputPort
    {
        Task ICreateMeasurementOutputPort.Success(Measurement measurement, CancellationToken cancellationToken)
        {
            var _Measurement = mapper.Map<MeasurementViewModel>(measurement);
            viewModel.Measurements.Add(store.UpdateOrRegister(_Measurement.MeasurementId, _Measurement));
            toastService.ShowToast(ToastType.Success, "Measurement Created", $"{measurement.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteMeasurementPresenter(ToastService toastService, IViewModelStore store, MeasurementsViewModel viewModel) : IDeleteMeasurementOutputPort
    {
        Task IDeleteMeasurementOutputPort.Failure(string reason, Measurement? measurement, CancellationToken cancellationToken)
        {
            toastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteMeasurementOutputPort.Success(Measurement deletedMeasurement, CancellationToken cancellationToken)
        {
            _ = viewModel.Measurements.RemoveAll(m => m.MeasurementId == deletedMeasurement.MeasurementId);
            store.Remove<MeasurementViewModel>(deletedMeasurement.MeasurementId);
            toastService.ShowToast(ToastType.Info, "Measurement Deleted", $"{deletedMeasurement.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetMeasurementsPresenter(IMapper mapper, IViewModelStore store, MeasurementsViewModel viewModel) : IGetMeasurementsOutputPort
    {
        Task IGetMeasurementsOutputPort.Success(List<Measurement> measurements, CancellationToken cancellationToken)
        {
            viewModel.Measurements = [.. mapper.Map<List<MeasurementViewModel>>(measurements).Select(m => store.UpdateOrRegister(m.MeasurementId, m))];
            return Task.CompletedTask;
        }
    }
}