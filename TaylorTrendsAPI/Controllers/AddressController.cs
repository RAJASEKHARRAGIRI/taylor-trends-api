using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaylorTrendsAPI.Models;
using TaylorTrendsAPI.Services;

namespace TaylorTrendsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _addressService.getAllAddresses();
            return Ok(products);
        }

        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress(Address address)
        {
            var response = await _addressService.AddAddress(address);
            return Ok(new
            {
                message = "Successfully Address Added.",
                id = response
            });
        }

        [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(Address address)
        {
            if (address.Id <= 0)
            {
                return BadRequest();
            }

            var response = await _addressService.UpdateAddress(address);
            return Ok(new
            {
                message = "Successfully Address Updated.",
                id = response
            });
        }

        [HttpDelete("{addressId:int}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            if (addressId <= 0)
            {
                return BadRequest();
            }

            var response = await _addressService.DeleteAddress(addressId);
            return Ok(new
            {
                message = "Successfully Address Deleted.",
                id = response
            });
        }
    }
}
