
using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace ClarabridgeChallenge.Repository.SqlStorage
{
    public abstract class RepositoryBase : IRepositoryBase<ModelBase>
    {
        public abstract ModelBase Get(Guid id);
        public abstract IList<ModelBase> GetAll();
        public abstract void Add(ModelBase model);
        public abstract void Update(ModelBase model);
        public abstract void Delete(Guid id);
        public abstract void DeleteAll();
    }
}
