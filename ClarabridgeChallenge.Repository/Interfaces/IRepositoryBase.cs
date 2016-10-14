using ClarabridgeChallenge.Models;
using System;
using System.Collections.Generic;

namespace ClarabridgeChallenge.Repository.Interfaces
{
    public interface IRepositoryBase<T> where T : ModelBase
    {
        void Add(T item);
        void Update(T item);
        void Delete(Guid id);
        void DeleteAll();
        T Get(Guid id);
        IList<T> GetAll();
    }
}
