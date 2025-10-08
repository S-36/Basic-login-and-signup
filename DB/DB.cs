using DotNetEnv;
using MongoDB.Driver;

namespace Login_and_Signup.DB
{

    public class NoSQL
    {
         public IMongoDatabase db; 
         public MongoClient Client;

        public NoSQL()
        {
            // load the env variables
            Env.Load();
            // get the variables
            var DB_NAME = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new InvalidOperationException("DB_NAME not found");
            var DB_CONECTION = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? throw new InvalidOperationException("DB_CONECTION not found");

            // load the variables to the MongoClient
            Client = new MongoClient(DB_CONECTION);
            db = Client.GetDatabase(DB_NAME);
        }
    }
}