using System.Windows.Markup;
using Login_and_Signup.DB;
using Login_and_Signup.JWT;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.model;
using MongoDB.Driver;

namespace Login_and_Signup.User.collection
{
    public class UserCollection : UserInterface
    {
        internal NoSQL _repository = new NoSQL();
        private IMongoCollection<UserModel> Collection;

        public UserCollection()
        {
            Collection = _repository.db.GetCollection<UserModel>("Users");
        }

        public async Task CreateUser(UserModel user)
        {
            if (user == null || user.email == null || user.password == null)
            {
                throw new ArgumentNullException("User or email or password is null");
            }

            var existingUser = await Collection.Find(u => u.email == user.email).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = hashedPassword;

            await Collection.InsertOneAsync(user);
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            return await Collection.Find(u => u.email == email).FirstOrDefaultAsync();
        }

        public async Task<string> LoginUser(string email, string password)
        {
            if (email == null || password == null)
            {
                throw new ArgumentNullException("Email or password is null");
            }

            var user = await GetUserByEmail(email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.password))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var jwtService = new JwtService(
                Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new InvalidOperationException("JWT_SECRET_KEY not found"),
                Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new InvalidOperationException("JWT_ISSUER not found"),
                Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new InvalidOperationException("JWT_AUDIENCE not found")
            );
            return jwtService.GenerateToken(user._id.ToString(), user.email);
        }
    }
}