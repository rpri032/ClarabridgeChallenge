using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ClarabridgeChallenge.Repository.NoSqlStorage
{
    public class PressReleaseRepository : RepositoryBase<IPressReleaseRepository, PressRelease>, IPressReleaseRepository
    {
        public override IList<ModelBase> GetAll()
        {
            return Collection.Find(new BsonDocument()).SortByDescending(mb => mb.DatePublished).ToListAsync().Result.ConvertAll(x => (ModelBase)x);
        }

    }
}
