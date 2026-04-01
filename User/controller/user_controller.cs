
using Login_and_Signup.User.dtos;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.model;
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
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                await _userService.RegisterAsync(request.name, request.email, request.password);
                
                // 201 Created es más semántico que 200 OK para creación de recursos
                return StatusCode(201, new { message = "User registered successfully." });
            }
            catch (ArgumentNullException ex)
            {
                // 400 — El cliente envió datos inválidos
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // 409 Conflict — El recurso ya existe
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // 500 — Error interno — no exponemos detalles al cliente
                _logger.LogError(ex, "Unexpected error during registration");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var token = await _userService.LoginAsync(request.email, request.password);
                return Ok(new { token });
            }
            catch (ArgumentNullException ex)
            {   // 400 - El Cliente envio datos invalidos
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {   // 401 - Credenciales incorrectas
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {   // 400 - El CLiente envio datos invalidos o credenciales incorrectos 
                _logger.LogError(ex, "Unexpected error during login");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}