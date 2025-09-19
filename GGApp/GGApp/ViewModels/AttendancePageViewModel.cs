using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using ReactiveUI;

namespace GGApp.ViewModels;

public class AttendancePageViewModel : ViewModelBase
{
    private string _studentName = "Загрузка...";
    
    public string StudentName 
    { 
        get => _studentName;
        set => this.RaiseAndSetIfChanged(ref _studentName, value);
    }
    
    public ObservableCollection<AttendanceRow> Rows { get; } = new();
    public ObservableCollection<string> DayHeaders { get; } = new();

    public AttendancePageViewModel()
    {
        Console.WriteLine("AttendancePageViewModel constructor");
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            Console.WriteLine("Loading data...");
            
            if (AppSession.CurrentUserId is null)
            {
                Console.WriteLine("No current user ID");
                return;
            }

            var studentId = AppSession.CurrentUserId.Value;
            using var db = new GGContext();

            // Получаем студента
            var student = db.Users.FirstOrDefault(u => u.Id == studentId);
            StudentName = student is null ? "Студент" : $"{student.Surname} {student.Name}".Trim();

            // Загружаем тестовые данные
            LoadTestData();
            
            Console.WriteLine("Data loaded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            LoadTestData();
        }
    }

    private void LoadTestData()
    {
        Console.WriteLine("Loading test data...");
        
        Rows.Clear();
        DayHeaders.Clear();

        // Текущий месяц для заголовков
        var currentDate = DateTime.Today;
        var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
        
        // Создаем заголовки с числами и днями недели
        for (int day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(currentDate.Year, currentDate.Month, day);
            var dayOfWeek = GetShortDayOfWeek(date.DayOfWeek);
            DayHeaders.Add($"{day}\n{dayOfWeek}");
        }

        // Создаем тестовые данные для 3 месяцев
        for (int monthOffset = -2; monthOffset <= 0; monthOffset++)
        {
            var monthDate = DateTime.Today.AddMonths(monthOffset);
            var monthDays = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);
            
            var values = new string[monthDays];
            var totalHours = 0;
            var random = new Random();
            
            for (int d = 0; d < monthDays; d++)
            {
                // Случайные данные для теста
                if (random.Next(0, 10) > 6) // 40% chance of having data
                {
                    var hours = random.Next(1, 5);
                    values[d] = hours.ToString();
                    totalHours += hours;
                }
                else
                {
                    values[d] = "0";
                }
            }
            
            Rows.Add(new AttendanceRow(
                GetRussianMonthYear(monthDate),
                values,
                totalHours
            ));
        }

        Console.WriteLine($"Test rows count: {Rows.Count}");
        Console.WriteLine($"Day headers count: {DayHeaders.Count}");
    }

    private string GetShortDayOfWeek(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "Пн",
            DayOfWeek.Tuesday => "Вт",
            DayOfWeek.Wednesday => "Ср",
            DayOfWeek.Thursday => "Чт",
            DayOfWeek.Friday => "Пт",
            DayOfWeek.Saturday => "Сб",
            DayOfWeek.Sunday => "Вс",
            _ => "??"
        };
    }

    private static readonly string[] RussianMonths =
    {
        "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
        "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    };

    private string GetRussianMonthYear(DateTime date)
    {
        try
        {
            return $"{RussianMonths[date.Month - 1]} {date:yyyy}";
        }
        catch
        {
            return date.ToString("yyyy-MM");
        }
    }
}