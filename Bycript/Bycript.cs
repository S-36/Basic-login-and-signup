namespace Login_and_Signup.Bycript
{
    public interface IBycriptService
    {
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string hashedPassword);
    }
    public class BycriptService : IBycriptService
    {
        public Task<string> HashPasswordAsync(string password)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
        }

        public Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, hashedPassword));
        }
    }
}