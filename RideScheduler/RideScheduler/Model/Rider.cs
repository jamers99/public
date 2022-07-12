namespace RideScheduler.Model
{
    public class Rider
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid GroupId { get; set; }
    }
}
