using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GGApp.ViewModels;
using ReactiveUI;

namespace GGApp.Views;

public partial class ProfilePage : ReactiveUserControl<ProfilePageViewModel>
{
    public ProfilePage()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
    }

}
