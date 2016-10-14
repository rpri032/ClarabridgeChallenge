using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ClarabridgeChallenge.Repository.LocalStorage
{
    public class PressReleaseRepository : RepositoryBase, IPressReleaseRepository
    {
        public override IList<ModelBase> GetAll()
        {
            IEnumerable<object> localStorageValues = LocalStorage.Where(kvp => kvp.Key.StartsWith(string.Format(LOCAL_STORAGE_KEY_FORMATTER_PREFIX, GetType().Name))).ToList().Select(kvp => kvp.Value).Distinct();   // Select just the keys
            return (new List<ModelBase>(localStorageValues.Cast<PressRelease>().OrderByDescending(pr => pr.DatePublished)));
        }
    }
}
