using MongoDB.Driver;
using System.Configuration;

namespace ClarabridgeChallenge.Repository.NoSqlStorage
{
    public class MongoDb
    {
        private static MongoDb _instance;
        public static MongoDb Instance
        {
            get { return _instance ?? (_instance = new MongoDb()); }
        }

        private const string MongoDbConnectionStringKey = "MongoDB_ConnectionString";
        private const string MongoDbDatabaseNameKey = "MongoDB_DatabaseName";
        private string _mongoDbConnectionString;
        private string MongoDbConnectionString
        {
            get { return _mongoDbConnectionString ?? (_mongoDbConnectionString = ConfigurationManager.AppSettings[MongoDbConnectionStringKey]); }
        }

        private string _mongoDbDatabaseName;
        private string MongoDbDatabaseName
        {
            get { return _mongoDbDatabaseName ?? (_mongoDbDatabaseName = ConfigurationManager.AppSettings[MongoDbDatabaseNameKey]); }
        }

        private readonly IMongoClient _mongoClient;
        private IMongoClient MongoClient { get { return _mongoClient; } }

        private readonly IMongoDatabase _mongoDatabase;
        public IMongoDatabase MongoDatabase { get { return _mongoDatabase; } }


        private MongoDb()
        {
            _mongoClient = new MongoClient(MongoDbConnectionString);
            _mongoDatabase = MongoClient.GetDatabase(MongoDbDatabaseName);
        }
    }

}
