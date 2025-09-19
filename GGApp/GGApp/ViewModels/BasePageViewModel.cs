using System;
using System.Windows.Input;
using ReactiveUI;

namespace GGApp.ViewModels;

public class BasePageViewModel : ReactiveObject, IRoutableViewModel, IScreen
{
    public BasePageViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        Router.Navigate.Execute(new TimetablePageViewModel(HostScreen));
    }
    
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();

    public ICommand NavigateCommand => ReactiveCommand.Create<string>((x) =>
    {
        switch (x)
        {
            case "tt":
                Router.Navigate.Execute(new TimetablePageViewModel(this));
                break;
            case "profile":
                Router.Navigate.Execute(new LoginPageViewModel(this));
                break;
        }
    });
}