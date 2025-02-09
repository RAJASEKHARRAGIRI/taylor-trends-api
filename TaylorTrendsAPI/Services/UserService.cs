using System.Data.SqlClient;
using TaylorTrendsAPI.Common;
using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseConfig _databaseConfig;
        public UserService(DatabaseConfig databaseConfig) 
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<User> getUserDetailsByMobileNumber(LoginRequest loginRequest)
        {
            var user = new User() { Age = 0, Id = -1, MobileNumber = "", Name="", CreatedAt= new DateTime()};
            try {
                using (var conn = _databaseConfig.GetConnection())
                {
                    var cmd = new SqlCommand($"select * from users where MobileNumber = @mobileNumber AND Password = @password", conn);
                    cmd.Parameters.AddWithValue("@mobileNumber", loginRequest.MobileNumber);
                    cmd.Parameters.AddWithValue("@password", loginRequest.Password);
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString() ?? string.Empty,
                                MobileNumber = reader["MobileNumber"].ToString() ?? string.Empty,
                                Age = Convert.ToInt32(reader["Age"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            };
                        }
                        else
                        {
                            return user;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        //private User MapToUserDetails(SqlDataReader reader)
        //{
        //    return new User
        //    {
        //        Id = Convert.ToInt32(reader["Id"]),
        //        Name = reader["Name"].ToString() ?? string.Empty,
        //        Age = Convert.ToInt32(reader["Rating"]),
        //        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
        //    };
        //}
    
    }
}
