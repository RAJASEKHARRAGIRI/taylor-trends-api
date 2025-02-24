namespace TaylorTrendsAPI.Models
{
    public class ProductFilterRequest
    {
        public int? Price { get; set; }
        public string? Category { get; set; } = string.Empty;
        public int? Discount { get; set; }

    }
}
