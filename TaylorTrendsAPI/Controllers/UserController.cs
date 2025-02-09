using Microsoft.AspNetCore.Mvc;
using TaylorTrendsAPI.Models;
using TaylorTrendsAPI.Services;

namespace TaylorTrendsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public UserController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginRequest loginRequest)
        {
            bool errorFlag = false;
            if (loginRequest.MobileNumber == null || loginRequest.Password == null)
            {
                return BadRequest();
            }
            var userDetails = await _userService.getUserDetailsByMobileNumber(loginRequest);
            
            if (userDetails == null || userDetails.Id == -1) 
            {
                return Unauthorized();
            }

            var response = _authenticationService.getAuthenticationToken(loginRequest, userDetails);
            return Ok(new
            {
                message = errorFlag ? "Login Failed, Please enter vaild credentials." : "Successfully Login",
                user = errorFlag ? null : userDetails,
                token = errorFlag ? "": response
            });
        }
    }
}
