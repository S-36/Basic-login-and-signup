using Login_and_Signup.User.collection;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.model;
using Microsoft.AspNetCore.Mvc;

namespace Login_and_Signup.User.controller
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private UserInterface db = new UserCollection();

        [HttpPost("signup")]
        public async Task<IActionResult> Register(UserModel user)
        {
            try
            {
                await db.CreateUser(user);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin user)
        {
            try
            {
                var token = await db.LoginUser(user.email, user.password);
                return Ok( new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}