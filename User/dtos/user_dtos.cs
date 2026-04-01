using System.ComponentModel.DataAnnotations;

namespace Login_and_Signup.User.dtos
{
    public class UserRegisterRequest
    {
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }

    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }
}