namespace RideScheduler.Model
{
    public class Rider
    {
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public Guid GroupId { get; set; }
        public string HashedPassword { get; internal set; } = "";
    }
}
