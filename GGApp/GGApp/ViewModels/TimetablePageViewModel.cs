using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace GGApp.ViewModels;

public class TimetablePageViewModel : ReactiveObject, IRoutableViewModel, IScreen, INotifyPropertyChanged
{
    private DateTime _date = DateTime.Today;

    public TimetablePageViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        UpdateTable();
    }

    public void UpdateTable()
    {
        TableData = App.Db.Lessons.ToList().Where(i => i.LessonDay.AddHours(3).Date == Date).Select(i =>
                new LessonViewModel(
                        App.Db.Grades.FirstOrDefault(j => j.LessonId == i.Id && j.StudentId == App.State.User.Id))
                    { Deadline = i.Deadline, Id = i.Id, LessonDay = i.LessonDay, Subject = i.Subject, Task = i.Task })
            .ToList();
        this.RaisePropertyChanged(nameof(TableData));
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            this.RaiseAndSetIfChanged(ref _date, value);
            UpdateTable();
            this.RaisePropertyChanged(nameof(Day));
            this.RaisePropertyChanged(nameof(Today));
        }
    }

    public string Day => Date.ToString("M");
    public bool Today => _date == DateTime.Today;

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();
    public List<LessonViewModel> TableData { get; private set; } = new();

    public ICommand NextDate => ReactiveCommand.Create(() =>
    {
        Date = Date.AddDays(1);
    });

    public ICommand LastDate => ReactiveCommand.Create(() =>
    {
        Date = Date.AddDays(-1);
    });
}

public class LessonViewModel : Lesson
{
    public Grade? Grade { get; set; }

    public string Time => this.LessonDay.ToString("t");
    
    public LessonViewModel(Grade? grade)
    {
        Grade = grade;
    }
}