using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Login_and_Signup.User.model
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        // Lista de roles Para solucionar la entrada a multiples niveles y por defecto el rol de User
        public List<string> roles { get; set; } = new List<string>() { "User" };
    }
}