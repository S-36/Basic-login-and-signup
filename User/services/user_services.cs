using Login_and_Signup.Bycript;
using Login_and_Signup.Error;
using Login_and_Signup.JWT;
using Login_and_Signup.User.dtos;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.model;


namespace Login_and_Signup.User.services
{
    public class UserService : IUserService
    {
        // Implementacion del Repositorio y Servicios de Logica a usar
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IBycriptService _bycriptService;
        public UserService(IUserRepository userRepository, IJwtService jwtService, IBycriptService bycriptService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _bycriptService = bycriptService;
        }

        public async Task<Result<string>> LoginAsync(UserLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return Result<string>.Failure("Email and password must be provided.", 400);
            }
            var user = await _userRepository.GetUserByEmail(request.email);
            if (user == null)
            {
                return Result<string>.Failure("User not found.", 404);
            }
            var isPasswordValid = await _bycriptService.VerifyPasswordAsync(request.password, user.password);
            if (!isPasswordValid)
            {
                return Result<string>.Failure("Invalid password.", 401);
            }
            var token = _jwtService.GenerateToken(user._id, request.password, user.roles);
            return Result<string>.Success(token);
        }

        public async Task<Result> RegisterAsync(UserRegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.name) || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return Result.Failure("Name, email, and password must be provided.", 400);
            }
            var existingUser = _userRepository.GetUserByEmail(request.email).Result;
            if (existingUser != null)
            {
                return Result.Failure("User with this email already exists.", 409);
            }
            var hashedPassword = _bycriptService.HashPasswordAsync(request.password).Result;
            var newUser = new UserModel
            {
                name = request.name,
                email = request.email,
                password = hashedPassword
            };
            await _userRepository.CreateUser(newUser);
            return Result.Success(201);
        }

        public async Task<Result> ValidateEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Result.Failure("Email must be provided.", 400);
            }
            var existingUser = _userRepository.GetUserByEmail(email).Result;
            if (existingUser != null)
            {
                return Result.Failure("Email is already registered.", 409);
            }
            return Result.Success(200);
        }
    }
}