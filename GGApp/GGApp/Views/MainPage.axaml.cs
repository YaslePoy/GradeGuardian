using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using GGApp.ViewModels;
using ReactiveUI;
using System;

namespace GGApp.Views;

public partial class MainPage : ReactiveUserControl<MainPageViewModel>
{
    public MainPage()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
    }

    private void LogoutButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Console.WriteLine("=== LOGOUT BUTTON CLICKED ===");
        
        if (this.GetVisualRoot() is Window win)
        {
            // Очищаем сессию
            AppSession.CurrentUserId = null;
            
            // Возвращаемся к странице входа
            var loginPage = new LoginPage { DataContext = new LoginPageViewModel(null) };
            win.Content = loginPage;
        }
    }
}
