using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace GGApp.ViewModels;

public class TimetablePageViewModel : ReactiveObject, IRoutableViewModel, IScreen
{
    public TimetablePageViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        UpdateTable();
    }

    public void UpdateTable()
    { 
        TableData =  App.Db.Lessons.ToList().Where(i => i.LessonDay.AddHours(3).Date == Date).Select(i => new LessonViewModel(App.Db.Grades.FirstOrDefault(j => j.LessonId == i.Id && j.StudentId == App.State.UserId))
            { Deadline = i.Deadline, Id = i.Id, LessonDay = i.LessonDay, Subject = i.Subject, Task = i.Task }).ToList();
        this.RaisePropertyChanged(nameof(TableData));
    }
    
    public DateTime Date => DateTime.Today;
    public string Day => Date.ToString("M");
    public bool Today => true;

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
    public RoutingState Router { get; } = new();
    public List<LessonViewModel> TableData { get; private set; } = new();
}

public class LessonViewModel : Lesson
{
    public Grade? Grade { get; set; }

    public LessonViewModel(Grade? grade)
    {
        Grade = grade;
    }
}