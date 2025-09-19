using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using ReactiveUI;

namespace GGApp.ViewModels;

public class LoginPageViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly List<string> _incorrectAuth = new();
    private bool _invalidData;

    public LoginPageViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public string Login { get; set; }
    public string Password { get; set; }
    

    public ICommand TryAuth => ReactiveCommand.Create(async () =>
    {
        Debug.WriteLine($"Login: {Login}, password: {Password}");
        if (App.Db.Users.FirstOrDefault(i => i.Login == Login && i.Password == Password) is {} loginResponse)
        {
            if (!Directory.Exists("./UserData")) Directory.CreateDirectory("./UserData");
            App.State.User = loginResponse;
            
            HostScreen.Router.Navigate.Execute(new TimetablePageViewModel(HostScreen));
        }
        else
        {
            _incorrectAuth.Add(Login + "+" + Password);
            this.RaiseAndSetIfChanged(ref _invalidData, true, nameof(InvalidData));
        }
    });

    public bool InvalidData
    {
        get => _invalidData;
        set => _invalidData = value;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
}