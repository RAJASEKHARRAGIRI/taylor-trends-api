using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public interface IUserService
    {
        Task<User> getUserDetailsByMobileNumber(LoginRequest loginRequest);
    }
}
