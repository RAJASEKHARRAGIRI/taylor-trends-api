using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public interface IAuthenticationService
    {
        string getAuthenticationToken(LoginRequest loginRequest, User user);
    }
}
