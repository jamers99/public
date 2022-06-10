namespace RideScheduler.Model
{
    public class Trip
    {
        User User { get; set; }

        DateTime Date { get; set; }
        DayOfWeek DayOfWeek { get; set; }

        TimeSpan ArriveBegin { get; set; }
        TimeSpan ArriveEnd { get; set; }

        TimeSpan ReturnBegin { get; set; }
        TimeSpan ReturnEnd { get; set; }
    }
}
