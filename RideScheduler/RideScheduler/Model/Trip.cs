namespace RideScheduler.Model
{
    public class Trip
    {
        public Guid Id { get; set; }
        public Rider User { get; set; }

        public DateTime Date { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan ArriveBegin { get; set; }
        public TimeSpan ArriveEnd { get; set; }

        public TimeSpan ReturnBegin { get; set; }
        public TimeSpan ReturnEnd { get; set; }
    }
}
