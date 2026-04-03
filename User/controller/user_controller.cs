
using Login_and_Signup.Error;
using Login_and_Signup.User.dtos;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login_and_Signup.User.controller
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
                var result = await _userService.RegisterAsync(request);
                if (!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode, new { error = result.Error });
                }
                return StatusCode(201, new { message = "User registered successfully." });
                
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
                var result = await _userService.LoginAsync(request);
                if (!result.IsSuccess)
                {
                    return StatusCode(result.StatusCode, new { error = result.Error });
                }
                return StatusCode(200, new { token = result.Value });
        }
    }
}