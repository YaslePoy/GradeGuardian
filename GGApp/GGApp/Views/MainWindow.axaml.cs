using Avalonia.Controls;
using Avalonia.Interactivity;
using GGApp.Views;
using GGApp.ViewModels;
using ReactiveUI;

namespace GGApp.Views;

public partial class MainWindow : Window, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    public MainWindow()
    {
        InitializeComponent();
        Content = new LoginPage { DataContext = new LoginPageViewModel(this) };
    }
}