using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
       
        public AuthenticationService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public string getAuthenticationToken(LoginRequest loginRequest, User user)
        {
            if(loginRequest.MobileNumber == null || loginRequest.Password == null)
            {
                throw new ArgumentNullException("username");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("MobileNumber", loginRequest.MobileNumber),
                new Claim("Name", user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(600),
                signingCredentials: signIn
                );
            string bearerToken = new JwtSecurityTokenHandler().WriteToken(token);
            return bearerToken;
        }
    }
}
