using Login_and_Signup.Bycript;
using Login_and_Signup.JWT;
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

        public async Task<string> LoginAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email and password must be provided.");
            }
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            var isPasswordValid = await _bycriptService.VerifyPasswordAsync(password, user.password);
            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Invalid password.");
            }
            return _jwtService.GenerateToken(user._id, user.email);
        }

        public Task RegisterAsync(string name, string email, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Name, email, and password must be provided.");
            }
            var existingUser = _userRepository.GetUserByEmail(email).Result;
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }
            var hashedPassword = _bycriptService.HashPasswordAsync(password).Result;
            var newUser = new UserModel
            {
                name = name,
                email = email,
                password = hashedPassword
            };
            return _userRepository.CreateUser(newUser);
        }

        public Task ValidateEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email must be provided.");
            }
            var existingUser = _userRepository.GetUserByEmail(email).Result;
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email is already registered.");
            }
            return Task.CompletedTask;
        }
    }
}