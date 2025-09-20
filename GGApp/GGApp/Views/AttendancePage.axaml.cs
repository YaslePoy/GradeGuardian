using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GGApp.ViewModels;
using Avalonia.Threading;

namespace GGApp.Views;

public partial class AttendancePage : UserControl
{
    private bool _isScrolling;
    
    public AttendancePage()
    {
        Console.WriteLine("=== AttendancePage CONSTRUCTOR called ===");
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void DataScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (_isScrolling) return;
        
        _isScrolling = true;
        
        var dataScrollViewer = sender as ScrollViewer;
        var headerScrollViewer = this.FindControl<ScrollViewer>("headerScrollViewer");
        
        if (dataScrollViewer != null && headerScrollViewer != null)
        {
            // Синхронизируем горизонтальную прокрутку
            headerScrollViewer.Offset = new Avalonia.Vector(dataScrollViewer.Offset.X, 0);
        }
        
        _isScrolling = false;
    }

    protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Console.WriteLine("Attached to visual tree");
    }
}