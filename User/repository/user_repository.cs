using Login_and_Signup.DB;
using Login_and_Signup.User.Interface;
using Login_and_Signup.User.model;
using MongoDB.Driver;

namespace Login_and_Signup.User.repository
{
    public class UserRepository  : IUserRepository
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        public UserRepository(IMongoContext context)
        {
            _userCollection = context.GetMongoCollection<UserModel>("Users");
        }

        public async Task CreateUser(UserModel user)
        {
            try
            {
                await _userCollection.InsertOneAsync(user);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new InvalidOperationException("A user with this email already exists.", ex);
            }
        }

        public async Task DeleteUser(string password)
        {
             await _userCollection.DeleteOneAsync(u => u.password == password);
        }

        public async Task<UserModel?> GetUserByEmail(string email)
        {
            return await _userCollection.Find(u => u.email == email).FirstOrDefaultAsync();
        }

        public async Task UpdateUser(UserModel user)
        {
            await _userCollection.UpdateOneAsync(
                u => u._id == user._id,
                Builders<UserModel>.Update
                    .Set(u => u.name, user.name)
                    .Set(u => u.email, user.email)
            );
        }
    }
}