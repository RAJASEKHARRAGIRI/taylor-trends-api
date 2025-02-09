using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> getAllAddresses();
        Task<int> AddAddress(Address address);
        Task<int> UpdateAddress(Address address);
        Task<int> DeleteAddress(int addressId);
    }
}
