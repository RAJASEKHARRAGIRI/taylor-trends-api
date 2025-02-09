using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using TaylorTrendsAPI.Common;
using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly DatabaseConfig _databaseConfig;

        public AddressService(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<int> DeleteAddress(int addressId)
        {
            if (addressId <= 0)
            {
                return -1;
            }

            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Delete from Addresses where Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", addressId);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> AddAddress(Address address)
        {
            if(CheckAddressExist(address.Id))
            {
                return await UpdateAddress(address);
            }

            if(address.IsDefault)
            {
                await RemoveDefaultAddress();
            }

            if (address.IsBilling)
            {
                await RemoveBillingAddress();
            }

            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Insert Into Addresses (FullName, Address1, Address2, City, State, PhoneNumber, LandMark, IsBilling, IsDefault, Pincode)" +
                                         " Values (@fullName, @address1, @address2, @city, @state, @phoneNumber, @landMark, @isBilling, @isDefault, @pincode)", conn);
                cmd.Parameters.AddWithValue("@fullName", address.FullName);
                cmd.Parameters.AddWithValue("@address1", address.Address1);
                cmd.Parameters.AddWithValue("@address2", address.Address2);
                cmd.Parameters.AddWithValue("@city", address.City);
                cmd.Parameters.AddWithValue("@state", address.State);
                cmd.Parameters.AddWithValue("@phoneNumber", address.PhoneNumber);
                cmd.Parameters.AddWithValue("@landMark", address.LandMark);
                cmd.Parameters.AddWithValue("@isBilling", address.IsBilling);
                cmd.Parameters.AddWithValue("@pincode", address.Pincode);
                cmd.Parameters.AddWithValue("@isDefault", address.IsDefault);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdateAddress(Address address)
        {
            if (address.Id <= 0 && !CheckAddressExist(address.Id))
            {
                return -1;
            }

            if (address.IsDefault)
            {
                await RemoveDefaultAddress();
            }

            if (address.IsBilling)
            {
                await RemoveBillingAddress();
            }

            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Update Addresses SET FullName=@fullName, Address1=@address1, Address2=@address2, City=@city, State=@state, Pincode = @pincode," +
                    "PhoneNumber=@phoneNumber, LandMark=@landMark, IsBilling=@isBilling, IsDefault=@isDefault WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@fullName", address.FullName);
                cmd.Parameters.AddWithValue("@address1", address.Address1);
                cmd.Parameters.AddWithValue("@address2", address.Address2);
                cmd.Parameters.AddWithValue("@city", address.City);
                cmd.Parameters.AddWithValue("@state", address.State);
                cmd.Parameters.AddWithValue("@phoneNumber", address.PhoneNumber);
                cmd.Parameters.AddWithValue("@landMark", address.LandMark);
                cmd.Parameters.AddWithValue("@pincode", address.Pincode);
                cmd.Parameters.AddWithValue("@isBilling", address.IsBilling);
                cmd.Parameters.AddWithValue("@isDefault", address.IsDefault);
                cmd.Parameters.AddWithValue("@id", address.Id);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Address>> getAllAddresses()
        {
            var addresses = new List<Address>();
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Select * from Addresses Order by IsDefault desc", conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        addresses.Add(MapToAddresses(reader));
                    }
                }
            }
            return addresses;
        }

        private Address MapToAddresses(SqlDataReader reader)
        {
            return new Address
            {
                Id = Convert.ToInt32(reader["ID"]),
                Pincode = Convert.ToInt32(reader["Pincode"]),
                FullName = reader["FullName"].ToString() ?? string.Empty,
                Address1 = reader["Address1"].ToString() ?? string.Empty,
                Address2 = reader["Address2"].ToString() ?? string.Empty,
                City = reader["City"].ToString() ?? string.Empty,
                State = reader["State"].ToString() ?? string.Empty,
                PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty,
                LandMark = reader["LandMark"].ToString() ?? string.Empty,
                IsBilling = Convert.ToBoolean(reader["IsBilling"]),
                IsDefault = Convert.ToBoolean(reader["IsDefault"]),
                Selected = Convert.ToBoolean(reader["IsDefault"]),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }

        private bool CheckAddressExist(int addressId)
        {
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Select Count(1) from Addresses Where Id = @addressId ", conn);
                cmd.Parameters.AddWithValue("@addressId", addressId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private async Task<int> RemoveDefaultAddress()
        {
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Update Addresses SET IsDefault=@isDefault", conn);
                cmd.Parameters.AddWithValue("@isDefault", false);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task<int> RemoveBillingAddress()
        {
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Update Addresses SET IsBilling=@isBilling", conn);
                cmd.Parameters.AddWithValue("@isBilling", false);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
