using System.Collections.ObjectModel;

namespace GGApp.ViewModels;

public class AttendanceRow
{
    public string MonthYear { get; set; } = "";
    public ObservableCollection<string> DayValues { get; set; } = new();
    public int TotalHours { get; set; }

    public AttendanceRow() { }

    public AttendanceRow(string monthYear, string[] dayValues, int totalHours)
    {
        MonthYear = monthYear;
        DayValues = new ObservableCollection<string>(dayValues);
        TotalHours = totalHours;
    }
}