namespace TaylorTrendsAPI.Models
{
    public class CartDetails
    {
        public required int CartId { get; set; }
        public required int ProductId { get; set; }
        public required int Quantity { get; set; }
        public required string Size { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public required string StockStatus { get; set; }
        public double Rating { get; set; }
        public required string Category { get; set; }
        public required string Brand { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
