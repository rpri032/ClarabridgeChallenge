using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;

namespace ClarabridgeChallenge.Repository.LocalStorage
{
    public abstract class RepositoryBase : IRepositoryBase<ModelBase>
    {
        protected const string LOCAL_STORAGE_KEY_FORMATTER_PREFIX = "{0}:";
        protected const string LOCAL_STORAGE_KEY_FORMATTER = LOCAL_STORAGE_KEY_FORMATTER_PREFIX + "{1}";
        private ObjectCache _localStorage;
        protected ObjectCache LocalStorage
        {
            get
            {
                return _localStorage = (_localStorage ?? MemoryCache.Default);
            }
        }

        private string GetStorageKey(Guid id)
        {
            return GetStorageKey(id.ToString());
        }

        private string GetStorageKey(string id)
        {
            return string.Format(LOCAL_STORAGE_KEY_FORMATTER, GetType().Name, id);
        }

        public ModelBase Get(Guid id)
        {
            var storageKey = GetStorageKey(id);
            if (LocalStorage.Contains(storageKey))
            {
                return LocalStorage[storageKey.ToString()] as ModelBase;
            }
            return null;
        }

        public abstract IList<ModelBase> GetAll();

        public void Add(ModelBase model)
        {
            AddUpdate(model);
        }

        public void Update(ModelBase model)
        {
            AddUpdate(model);
        }

        private void AddUpdate(ModelBase model)
        {
            LocalStorage[GetStorageKey(model.Id)] = model;
        }
        public void Delete(Guid id)
        {
            var storageKey = GetStorageKey(id);
            if (LocalStorage.Contains(storageKey))
            {
                LocalStorage.Remove(storageKey);
            }
        }

        public void DeleteAll()
        {
            List<string> ids = LocalStorage.Select(kvp => kvp.Key).Where(k => k.StartsWith(string.Format(LOCAL_STORAGE_KEY_FORMATTER_PREFIX, GetType().Name))).ToList();
            foreach (string id in ids)
            {
                LocalStorage.Remove(id);
            }
        }
    }
}
