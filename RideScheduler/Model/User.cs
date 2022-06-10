namespace RideScheduler.Model
{
    public class User
    {
        public string Name { get; set; }

        public List<Trip> WeeklyTrips { get; } = new List<Trip>();
    }
}
