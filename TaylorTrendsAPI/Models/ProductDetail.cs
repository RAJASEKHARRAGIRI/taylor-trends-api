namespace TaylorTrendsAPI.Models
{
    public class ProductDetail
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public required string StockStatus { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public required string Category { get; set; }
        public required string Brand { get; set; }
        public required List<Image> Images { get; set; }
        public int ImageId { get; set; }
        public string? Url { get; set; }
        public List<string>? Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
