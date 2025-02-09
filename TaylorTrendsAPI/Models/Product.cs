namespace TaylorTrendsAPI.Models
{
    public class Product
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
        public required List<string> Images { get; set; }    
        public List<string>? Tags { get; set; }   
        public DateTime CreatedAt { get; set; }    
        public DateTime UpdatedAt { get; set; }
    }   
}
