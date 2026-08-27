using CocktailCollator.Web.Common.State;

namespace CocktailCollator.Web.ViewModels.Measurements;

public class MeasurementViewModel : IStoreableViewModel<MeasurementViewModel>
{
    public required Guid MeasurementId { get; set; }
    public string? Name { get; set; }

    public void ApplyChanges(MeasurementViewModel source, IViewModelStore store)
    {
        this.Name = source.Name;
    }
}
