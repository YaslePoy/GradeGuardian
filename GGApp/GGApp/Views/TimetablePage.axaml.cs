using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using GGApp.ViewModels;

namespace GGApp.Views;

public partial class TimetablePage : ReactiveUserControl<TimetablePageViewModel>
{
    public TimetablePage()
    {
        InitializeComponent();
        
    }
}