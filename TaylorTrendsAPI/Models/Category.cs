namespace TaylorTrendsAPI.Models
{
    public class Category
    {
        public required int Id { get; set; }            
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }     // Creation timestamp
    }
}
