using ClarabridgeChallenge.Business.Responses;
using ClarabridgeChallenge.Business.Validation;
using ClarabridgeChallenge.Models;
using ClarabridgeChallenge.Repository.Interfaces;
using ClarabridgeChallenge.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClarabridgeChallenge.Business
{
    public class BusinessBase<R, M, V> 
        where R : IRepositoryBase<ModelBase>
        where M : Models.ModelBase
        where V : IValidationBase, new()
    {
        private static IRepositoryBase<ModelBase> _repositoryBase;
        protected static IRepositoryBase<ModelBase> RepositoryBase
        {
            get
            {
                return _repositoryBase = (_repositoryBase ?? UnityDependencyFactory.ResolveUnityContainer<R>());
            }
        }

        public ResponseInfoDetail Get(Guid id)
        {
            var item = RepositoryBase.Get(id);
            if (item != null)
            {
                return new ResponseInfoDetail() { Item = item, Status = ResponseStatus.Success };
            }
            return new ResponseInfoDetail() { Errors = new List<Error>() { new Error() { Message = String.Format(Errors.RecordDoesNotExist, id) } }, Status = ResponseStatus.Failed };
        }

        public ResponseInfoList GetAll()
        {
            var items = RepositoryBase.GetAll();
            return new ResponseInfoList() { Items = items, Status = ResponseStatus.Success };
        }

        public ResponseInfoDetail Add(M model)
        {
            var errors = new V().Validate(model, ValidationType.Add);
            if (!errors.Any())
            {
                model.Id = Guid.NewGuid();
                model.DateCreated = DateTime.Now;
                RepositoryBase.Add(model);
                return new ResponseInfoDetail() { Item = model, Status = ResponseStatus.Success };
            }
            return new ResponseInfoDetail() { Errors = errors, Status = ResponseStatus.Failed };
        }

        public ResponseInfoDetail Update(M model)
        {
            var errors = new V().Validate(model, ValidationType.Update);
            if (!errors.Any())
            {
                model.DateUpdated = DateTime.Now;
                RepositoryBase.Update(model);
                return new ResponseInfoDetail() { Item = model, Status = ResponseStatus.Success };
            }
            return new ResponseInfoDetail() { Errors = errors, Status = ResponseStatus.Failed };
        }

        public ResponseInfoDelete Delete(Guid id)
        {
            if (id != Guid.Empty && RepositoryBase.Get(id) != null)
            {
                RepositoryBase.Delete(id);
                return new ResponseInfoDelete() { Status = ResponseStatus.Success };
            }
            return new ResponseInfoDelete() { Errors = new List<Error>() { new Error() { Message = String.Format(Errors.RecordDoesNotExist, id) } }, Status = ResponseStatus.Failed };
        }

        public ResponseInfoDelete DeleteAll()
        {
            RepositoryBase.DeleteAll();
            return new ResponseInfoDelete() { Status = ResponseStatus.Success };
        }
    }
}
