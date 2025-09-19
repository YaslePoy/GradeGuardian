using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using Avalonia.Threading;
using GGApp.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using System;
using System.Threading.Tasks;

namespace GGApp.Views;

public partial class LoginPage : ReactiveUserControl<LoginPageViewModel>
{
    private const double Button_VISIBLE = 39;

    public LoginPage()
    {
        this.WhenActivated(_ => { });
        InitializeComponent();
    }

    private void UpdateLoginVisibility(object? sender, TextChangedEventArgs e)
    {
        LoginButton.Height = string.IsNullOrWhiteSpace(LoginText.Text) ||
                             string.IsNullOrWhiteSpace(PasswordText.Text) || PasswordText.Text.Length < 6
            ? 0
            : Button_VISIBLE;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (this.GetVisualRoot() is Window win)
        {
            // Подписываемся на успешную авторизацию и выполняем переход
            if (DataContext is LoginPageViewModel vm)
            {
                // Подписываемся на завершение команды TryAuth
                vm.TryAuth.IsExecuting
                    .Skip(1) // Пропускаем начальное значение
                    .Where(executing => !executing) // Когда команда завершилась
                    .Subscribe(async _ =>
                    {
                        // Даем время на установку CurrentUserId
                        await Task.Delay(100);
                        
                        if (AppSession.CurrentUserId.HasValue)
                        {
                            Dispatcher.UIThread.Post(() =>
                            {
                                Console.WriteLine($">>> SWITCHING TO MAIN PAGE FOR USER {AppSession.CurrentUserId} <<<");
                                win.Content = new MainPage { DataContext = new MainPageViewModel() };
                            }, DispatcherPriority.Background);
                        }
                    });
            }
        }
    }
}