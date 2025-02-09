namespace TaylorTrendsAPI.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required string MobileNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
