using DotNetEnv;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Login_and_Signup.DB
{

    public class MongoSettings 
    {   
        public string ConectionString {get; set;} = string.Empty;
        public string DatabaseName {get; set;} = string.Empty;
    }

    public interface IMongoContext
    {
        IMongoCollection<T> GetMongoCollection<T>(string collectionName);
    }
    public class MongoDBContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        // Se utiliza Ioptions que es la forma nativa de .NET Para recibir configuracion tipada desde Program.cs
        public MongoDBContext(IOptions<MongoSettings> settings)
        {
            
            var client = new MongoClient(settings.Value.ConectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        } 
    }
}