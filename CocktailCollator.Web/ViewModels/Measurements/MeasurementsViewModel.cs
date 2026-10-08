using AutoMapper;
using CocktailCollator.Application.UseCases.Measurements.CreateMeasurement;
using CocktailCollator.Application.UseCases.Measurements.DeleteMeasurement;
using CocktailCollator.Application.UseCases.Measurements.GetMeasurements;
using CocktailCollator.Domain.Entities;
using CocktailCollator.UseCasePipelines.Pipes;
using CocktailCollator.Web.Common.Presenters;
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
        IPipeline<CreateMeasurementInputPort, ICreateMeasurementOutputPort> createMeasurementPipeline,
        IPipeline<DeleteMeasurementInputPort, IDeleteMeasurementOutputPort> deleteMeasurementPipeline,
        IPipeline<IGetMeasurementsOutputPort> getMeasurementsPipeline,
        IMapper mapper,
        IViewModelStore store,
        ToastService toastService)
    {
        this.CreateCommand = new AsyncRelayCommand<CreateMeasurementInputPort>((inputPort, cancellationToken)
            => createMeasurementPipeline.ExecuteAsync(
                 inputPort,
                new CreateMeasurementPresenter(mapper, store, toastService, this),
                cancellationToken));

        this.DeleteCommand = new AsyncRelayCommand<Guid>((measurementId, cancellationToken)
            => deleteMeasurementPipeline.ExecuteAsync(
                new() { MeasurementId = measurementId },
                new DeleteMeasurementPresenter(store, toastService, this),
                cancellationToken));

        this.GetCommand = new AsyncRelayCommand(cancellationToken
            => getMeasurementsPipeline.ExecuteAsync(
                new GetMeasurementsPresenter(mapper, store, toastService, this),
                cancellationToken));
    }

    private class CreateMeasurementPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, MeasurementsViewModel viewModel)
        : BasePresenter(toastService, "create measurements"), ICreateMeasurementOutputPort
    {
        Task ICreateMeasurementOutputPort.Success(Measurement measurement, CancellationToken cancellationToken)
        {
            var _Measurement = mapper.Map<MeasurementViewModel>(measurement);
            viewModel.Measurements.Add(store.UpdateOrRegister(_Measurement.MeasurementId, _Measurement));
            this.ToastService.ShowToast(ToastType.Success, "Measurement Created", $"{measurement.Name} created successfully");
            return Task.CompletedTask;
        }
    }

    private class DeleteMeasurementPresenter(IViewModelStore store, ToastService toastService, MeasurementsViewModel viewModel)
        : BasePresenter(toastService, "delete measurements"), IDeleteMeasurementOutputPort
    {
        Task IDeleteMeasurementOutputPort.StillInUse(string reason, Measurement? measurement, CancellationToken cancellationToken)
        {
            this.ToastService.ShowToast(ToastType.Danger, "Failed to Delete", reason);
            return Task.CompletedTask;
        }

        Task IDeleteMeasurementOutputPort.Success(Measurement deletedMeasurement, CancellationToken cancellationToken)
        {
            _ = viewModel.Measurements.RemoveAll(m => m.MeasurementId == deletedMeasurement.MeasurementId);
            store.Remove<MeasurementViewModel>(deletedMeasurement.MeasurementId);
            this.ToastService.ShowToast(ToastType.Info, "Measurement Deleted", $"{deletedMeasurement.Name} deleted successfully");
            return Task.CompletedTask;
        }
    }

    private class GetMeasurementsPresenter(IMapper mapper, IViewModelStore store, ToastService toastService, MeasurementsViewModel viewModel)
        : BasePresenter(toastService, "view measurements"), IGetMeasurementsOutputPort
    {
        Task IGetMeasurementsOutputPort.Success(List<Measurement> measurements, CancellationToken cancellationToken)
        {
            viewModel.Measurements = [.. mapper.Map<List<MeasurementViewModel>>(measurements).Select(m => store.UpdateOrRegister(m.MeasurementId, m))];
            return Task.CompletedTask;
        }
    }
}