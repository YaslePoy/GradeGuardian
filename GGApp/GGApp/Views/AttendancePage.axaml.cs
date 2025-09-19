using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GGApp.ViewModels;
using Avalonia.Threading;

namespace GGApp.Views;

public partial class AttendancePage : UserControl
{
    public AttendancePage()
    {
        Console.WriteLine("=== AttendancePage CONSTRUCTOR called ===");
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        Console.WriteLine("DataContext changed");

        if (DataContext is AttendancePageViewModel vm)
        {
            Dispatcher.UIThread.InvokeAsync(() => 
            {
                Console.WriteLine($"Data loaded: {vm.Rows.Count} rows");
            }, DispatcherPriority.Loaded);
        }
    }

    private void DebugButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("=== REFRESH BUTTON CLICKED ===");
        
        var itemsControl = this.FindControl<ItemsControl>("itemsControl");
        if (itemsControl != null && DataContext is AttendancePageViewModel vm)
        {
            // Принудительно обновляем данные
            itemsControl.ItemsSource = null;
            itemsControl.ItemsSource = vm.Rows;
            
            Console.WriteLine($"ItemsControl refreshed with {vm.Rows.Count} rows");
        }
    }


    protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Console.WriteLine("Attached to visual tree");
    }
}