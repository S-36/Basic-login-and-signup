using MongoDB.Bson.Serialization.Attributes;

namespace Login_and_Signup.User.model
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class UserLogin
    {
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}