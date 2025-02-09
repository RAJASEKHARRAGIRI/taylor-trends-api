namespace TaylorTrendsAPI.Models
{
    public class LoginRequest
    {
        public required string MobileNumber { get; set; }
        public required string Password { get; set; }
    }
}
