using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ClarabridgeChallenge.Repository.NoSqlStorage
{
    public abstract class RepositoryBase<R, M> : IRepositoryBase<ModelBase> where R: IRepositoryBase<ModelBase> where M: ModelBase
    {
        private IMongoCollection<M> _collection;
        internal IMongoCollection<M> Collection
        {
            get
            {
                if (_collection == null)
                {
                    var mongoClient = new MongoClient(MongoDbConnectionString);
                    var mongoDatabase = mongoClient.GetDatabase(MongoDbDatabaseName);
                    _collection = mongoDatabase.GetCollection<M>(typeof(M).FullName);
                }
                return _collection;
            }
        }

        #region MongoDB Configuration
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

        internal interface IIdentified
        {
            ObjectId Id { get; }
        }
        #endregion
        
        public abstract IList<ModelBase> GetAll();

        public  ModelBase Get(Guid id)
        {
            var results = Collection.Find(mb => mb.Id == id).ToListAsync().Result;
            if (results.Count == 0)
            {
                return null;
            }
            return results[0];
        }

        public void Add(ModelBase modelBase)
        {
            M modelBaseM = (M)modelBase;
            Collection.ReplaceOne(dto => dto.Id == modelBaseM.Id,
                modelBaseM,
                new UpdateOptions { IsUpsert = true });
        }

        public void Update(ModelBase modelBase)
        {
            M modelBaseM = (M)modelBase;
            Collection.ReplaceOne(dto => dto.Id == modelBaseM.Id,
                modelBaseM,
                new UpdateOptions { IsUpsert = true });
        }

        public void Delete(Guid id)
        {
            Collection.DeleteMany(dto => dto.Id == id);
        }

        public void DeleteAll()
        {
            Collection.DeleteMany(new BsonDocument());
        }
    }
}
