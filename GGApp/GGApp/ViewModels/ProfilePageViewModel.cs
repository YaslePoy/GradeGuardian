using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using Microsoft.EntityFrameworkCore;

namespace GGApp.ViewModels;

public class ProfilePageViewModel : ViewModelBase
{
    private string _name = string.Empty;
    private string _surname = string.Empty;
    private string _email = string.Empty;
    private string _phone = string.Empty;
    private string _login = string.Empty;
    private string _password = string.Empty;
    private bool _isEditing = false;
    private string _tempEmail = string.Empty;
    private string _tempPhone = string.Empty;

    public ProfilePageViewModel()
    {
        LoadUserData();
        
        EditCommand = ReactiveCommand.Create(() =>
        {
            IsEditing = true;
            TempEmail = Email;
            TempPhone = Phone;
        });
        
        SaveCommand = ReactiveCommand.Create(async () =>
        {
            await SaveChanges();
            IsEditing = false;
        });
        
        CancelCommand = ReactiveCommand.Create(() =>
        {
            IsEditing = false;
            TempEmail = Email;
            TempPhone = Phone;
        });
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Surname
    {
        get => _surname;
        set => this.RaiseAndSetIfChanged(ref _surname, value);
    }

    public string FullName => $"{Surname} {Name}";

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }

    public string Phone
    {
        get => _phone;
        set => this.RaiseAndSetIfChanged(ref _phone, value);
    }

    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => this.RaiseAndSetIfChanged(ref _isEditing, value);
    }

    public string TempEmail
    {
        get => _tempEmail;
        set => this.RaiseAndSetIfChanged(ref _tempEmail, value);
    }

    public string TempPhone
    {
        get => _tempPhone;
        set => this.RaiseAndSetIfChanged(ref _tempPhone, value);
    }

    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private async void LoadUserData()
    {
        if (AppSession.CurrentUserId.HasValue)
        {
            try
            {
                using var context = new GGContext();
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == AppSession.CurrentUserId.Value);
                
                if (user != null)
                {
                    // Name = user.Name ?? string.Empty;
                    Surname = $"{user.Surname ?? string.Empty} {user.Name ?? string.Empty}";
                    Email = user.Email ?? string.Empty;
                    Phone = user.Phone ?? string.Empty;
                    Login = user.Login ?? string.Empty;
                    Password = new string('*', user.Password?.Length ?? 0);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки загрузки данных
                Console.WriteLine($"Ошибка загрузки данных пользователя: {ex.Message}");
            }
        }
    }

    private async System.Threading.Tasks.Task SaveChanges()
    {
        if (AppSession.CurrentUserId.HasValue)
        {
            try
            {
                using var context = new GGContext();
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == AppSession.CurrentUserId.Value);
                
                if (user != null)
                {
                    user.Email = TempEmail;
                    user.Phone = TempPhone;
                    
                    await context.SaveChangesAsync();
                    
                    Email = TempEmail;
                    Phone = TempPhone;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки сохранения
                Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
            }
        }
    }
}
