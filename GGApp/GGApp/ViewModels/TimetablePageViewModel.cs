using System;
using ReactiveUI;

namespace GGApp.ViewModels;

public class TimetablePageViewModel : ReactiveObject, IRoutableViewModel, IScreen
{
    public TimetablePageViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }
    public string Day => "";
    public bool Today => true;
    
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();
}