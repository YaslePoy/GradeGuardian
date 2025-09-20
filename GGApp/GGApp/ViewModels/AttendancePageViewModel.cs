using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using Microsoft.EntityFrameworkCore;

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
    public ObservableCollection<int> DayHeaders { get; } = new();

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

            var student = db.Users.FirstOrDefault(u => u.Id == studentId);
            StudentName = student is null ? "Студент" : $"{student.Surname} {student.Name}".Trim();

            LoadAttendanceData(db, studentId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
            LoadTestData();
        }
    }

    private void LoadAttendanceData(GGContext db, int studentId)
    {
        Rows.Clear();
    DayHeaders.Clear();

    // Получаем ВСЕ пропуски студента (IsPass = true) с датами
    var attendances = db.Attendences
        .Where(a => a.StudentId == studentId && a.IsPass)
        .Include(a => a.Lesson)
        .ToList(); // ✅ Убрали фильтр на null — он не нужен, если LessonDay не nullable

    Console.WriteLine($"Total attendances found: {attendances.Count}");

    if (!attendances.Any())
    {
        LoadEmptyData();
        return;
    }

    var monthsWithAttendance = attendances
        .Select(a => new
        {
            Year = a.Lesson.LessonDay.Year,   // ✅ Без .Value, потому что LessonDay — DateTime
            Month = a.Lesson.LessonDay.Month
        })
        .Distinct()
        .OrderBy(x => x.Year)
        .ThenBy(x => x.Month)
        .ToList();


        Console.WriteLine($"Months with attendance: {monthsWithAttendance.Count}");

        // Для каждого месяца генерируем строку
        foreach (var monthInfo in monthsWithAttendance)
        {
            var monthDate = new DateTime(monthInfo.Year, monthInfo.Month, 1);
            var monthDays = DateTime.DaysInMonth(monthInfo.Year, monthInfo.Month);

            // Обновляем DayHeaders — по дням текущего месяца
            DayHeaders.Clear();
            for (int day = 1; day <= monthDays; day++)
            {
                DayHeaders.Add(day);
            }

            var values = new string[monthDays];
            var totalHours = 0;

            for (int day = 1; day <= monthDays; day++)
            {
                var currentDay = new DateTime(monthInfo.Year, monthInfo.Month, day);
                
                // Воскресенье — тире
                if (currentDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    values[day - 1] = "—";
                    continue;
                }

                // Ищем пропуски в этот день
                var dayAttendances = attendances
                    .Where(a => a.Lesson.LessonDay.Date == currentDay.Date)
                    .ToList();

                if (dayAttendances.Count > 0)
                {
                    var hours = dayAttendances.Count * 2; // 2 часа за каждый пропуск
                    values[day - 1] = hours.ToString();
                    totalHours += hours;
                }
                else
                {
                    values[day - 1] = "0";
                }
            }
            
            Rows.Add(new AttendanceRow(
                GetRussianMonthYear(monthDate),
                values,
                totalHours
            ));
        }
    }

    private void LoadEmptyData()
    {
        Rows.Clear();
        DayHeaders.Clear();

        // Можно показать один месяц с нулями или просто сообщение
        var currentDate = DateTime.Today;
        var monthDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

        for (int day = 1; day <= monthDays; day++)
        {
            DayHeaders.Add(day);
        }

        var values = new string[monthDays];
        for (int i = 0; i < monthDays; i++)
        {
            values[i] = "0";
        }

        Rows.Add(new AttendanceRow(
            GetRussianMonthYear(currentDate),
            values,
            0
        ));

        Console.WriteLine("No attendance data — showing empty month");
    }

    private void LoadTestData()
    {
        Rows.Clear();
        DayHeaders.Clear();

        // Создаем тестовые "пропуски" только в 3 месяцах: например, февраль, март, апрель
        var testMonths = new[]
        {
            new DateTime(DateTime.Today.Year, 2, 1),
            new DateTime(DateTime.Today.Year, 3, 1),
            new DateTime(DateTime.Today.Year, 4, 1)
        };

        foreach (var monthDate in testMonths)
        {
            var monthDays = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);

            DayHeaders.Clear();
            for (int day = 1; day <= monthDays; day++)
            {
                DayHeaders.Add(day);
            }

            var values = new string[monthDays];
            var totalHours = 0;
            var random = new Random(monthDate.Month * 1000 + monthDate.Year); // для разнообразия

            for (int day = 1; day <= monthDays; day++)
            {
                var currentDay = new DateTime(monthDate.Year, monthDate.Month, day);
                
                if (currentDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    values[day - 1] = "—";
                    continue;
                }

                // Генерируем пропуски только в 60% дней (чтобы не все месяцы были пустыми)
                if (random.Next(0, 10) > 4)
                {
                    var hours = random.Next(1, 5);
                    values[day - 1] = hours.ToString();
                    totalHours += hours;
                }
                else
                {
                    values[day - 1] = "0";
                }
            }
            
            Rows.Add(new AttendanceRow(
                GetRussianMonthYear(monthDate),
                values,
                totalHours
            ));
        }
    }

    private static readonly string[] RussianMonths =
    {
        "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
        "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    };

    private string GetRussianMonthYear(DateTime date)
    {
        return $"{RussianMonths[date.Month - 1]} {date:yyyy}";
    }
}