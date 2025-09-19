using ReactiveUI;

namespace GGApp.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public MainWindowViewModel()
    {
        Router.Navigate.Execute(new TimetablePageViewModel(this));
    }
    public RoutingState Router { get; } = new();
}