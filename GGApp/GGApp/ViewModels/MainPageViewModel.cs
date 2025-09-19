using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using Microsoft.EntityFrameworkCore;
using GGApp.Views;

namespace GGApp.ViewModels;

public class MainPageViewModel : ViewModelBase
{
    private string _userName = string.Empty;
    private string _userSurname = string.Empty;
    private object _currentPage;
    private bool _isProfileSelected = true;
    private bool _isAttendanceSelected = false;

    public MainPageViewModel()
    {
        LoadUserData();
        
        // Инициализируем с страницей профиля
        CurrentPage = new ProfilePage { DataContext = new ProfilePageViewModel() };
        
        ProfileCommand = ReactiveCommand.Create(() =>
        {
            CurrentPage = new ProfilePage { DataContext = new ProfilePageViewModel() };
            IsProfileSelected = true;
            IsAttendanceSelected = false;
        });
        
        AttendanceCommand = ReactiveCommand.Create(() =>
        {
            CurrentPage = new AttendancePage { DataContext = new AttendancePageViewModel() };
            IsProfileSelected = false;
            IsAttendanceSelected = true;
        });
    }

    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    public string UserSurname
    {
        get => _userSurname;
        set => this.RaiseAndSetIfChanged(ref _userSurname, value);
    }

    public string FullName => $"{UserSurname} {UserName}";

    public object CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public bool IsProfileSelected
    {
        get => _isProfileSelected;
        set => this.RaiseAndSetIfChanged(ref _isProfileSelected, value);
    }

    public bool IsAttendanceSelected
    {
        get => _isAttendanceSelected;
        set => this.RaiseAndSetIfChanged(ref _isAttendanceSelected, value);
    }

    public ICommand ProfileCommand { get; }
    public ICommand AttendanceCommand { get; }
    public ICommand TimetableCommand => ReactiveCommand.Create(() => CurrentPage = new TimetablePage() { DataContext = new TimetablePageViewModel(null)});

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
                    // UserName = user.Name ?? string.Empty;
                    UserSurname = $"{user.Surname ?? string.Empty} {user.Name ?? string.Empty}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки данных пользователя: {ex.Message}");
            }
        }
    }
}
