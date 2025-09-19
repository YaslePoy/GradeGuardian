using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;

namespace GGApp.ViewModels;

public class LoginPageViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly List<string> _incorrectAuth = new();
    private bool _invalidData;

    public string Login { get; set; }
    public string Password { get; set; }
    
    public ReactiveCommand<Unit, Unit> TryAuth { get; }

    public LoginPageViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        
        TryAuth = ReactiveCommand.CreateFromTask(async () =>
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                _incorrectAuth.Add(Login + "+" + Password);
                this.RaiseAndSetIfChanged(ref _invalidData, true, nameof(InvalidData));
                return;
            }

            try
            {
                // РЕАЛЬНАЯ ПРОВЕРКА АВТОРИЗАЦИИ ИЗ БАЗЫ ДАННЫХ
                using var db = new GGContext();
                
                var loginInput = Login.Trim();
                var passwordInput = Password.Trim();
                var loginLower = loginInput.ToLower();

                // Ищем пользователя по логину или email
                var user = db.Users.FirstOrDefault(u =>
                    (u.Login.ToLower() == loginLower || u.Email.ToLower() == loginLower) &&
                    u.Password == passwordInput);

                if (user != null)
                {
                    // УСПЕШНАЯ АВТОРИЗАЦИЯ
                    AppSession.CurrentUserId = user.Id;
                    App.State.User = user;
                    Console.WriteLine($"Успешная авторизация: UserId={user.Id}, Name={user.Name} {user.Surname}");
                    
                    this.RaiseAndSetIfChanged(ref _invalidData, false, nameof(InvalidData));
                }
                else
                {
                    // НЕУСПЕШНАЯ АВТОРИЗАЦИЯ
                    _incorrectAuth.Add(Login + "+" + Password);
                    this.RaiseAndSetIfChanged(ref _invalidData, true, nameof(InvalidData));
                    Console.WriteLine("Неверные логин или пароль");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                _incorrectAuth.Add(Login + "+" + Password);
                this.RaiseAndSetIfChanged(ref _invalidData, true, nameof(InvalidData));
            }
        });
    }

    public bool InvalidData
    {
        get => _invalidData;
        set => _invalidData = value;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
}