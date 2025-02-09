namespace TaylorTrendsAPI.Models
{
    public class Address
    {
        public required int Id { get; set; }             
        public required string FullName { get; set; }          
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? LandMark { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsBilling { get; set; }
        public bool IsDefault{ get; set; }
        public bool Selected { get; set; }
        public required int Pincode { get; set; }
        public DateTime CreatedAt { get; set; }    
    }   
}
