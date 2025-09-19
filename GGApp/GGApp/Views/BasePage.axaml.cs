using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GGApp.ViewModels;

namespace GGApp.Views;

public partial class BasePage : ReactiveUserControl<BasePageViewModel>
{
    public BasePage()
    {
        InitializeComponent();
    }
}