using Login_and_Signup.User.model;

namespace Login_and_Signup.User.Interface
{
    public interface UserInterface
    {
        // Method to create a new user 
        Task CreateUser(UserModel user);
        // Method to login an return the JWT token
        Task<string> LoginUser(string email, string password);



        // metod to get a user by email
        Task<UserModel?> GetUserByEmail(string email);
    }

}