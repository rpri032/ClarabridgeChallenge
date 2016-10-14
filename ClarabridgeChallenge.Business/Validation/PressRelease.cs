using ClarabridgeChallenge.Repository.Interfaces;
using ClarabridgeChallenge.Resources;
using System;
using System.Collections.Generic;

namespace ClarabridgeChallenge.Business.Validation
{
    public class PressRelease : IValidationBase
    {
        private const int TitleLength = 50;

        private static IRepositoryBase<Models.ModelBase> _repository;
        protected static IRepositoryBase<Models.ModelBase> PressReleaseRepository
        {
            get
            {
                return _repository = (_repository ?? UnityDependencyFactory.ResolveUnityContainer<IPressReleaseRepository>());
            }
        }

        public IList<Error> Validate(Models.ModelBase modelBase, ValidationType validationType)
        {
            var pressRelease = (Models.PressRelease)modelBase;
            var errors = new List<Error>();
            if(validationType == ValidationType.Add)
            {
                if (pressRelease.Id != Guid.Empty)
                {
                    errors.Add(
                        new Error
                        {
                            Message = String.Format(Errors.RecordMustHaveEmptyId, pressRelease.Id)
                        });
                }
            }
            else
            {
                if (pressRelease.Id == Guid.Empty || PressReleaseRepository.Get(pressRelease.Id) == null)
                {
                    errors.Add(
                        new Error
                        {
                            Message = String.Format(Errors.RecordDoesNotExist, pressRelease.Id)
                        });
                }
            }

            if (String.IsNullOrEmpty(pressRelease.Title))
            {
                errors.Add(
                    new Error
                    {
                        Message = String.Format(Errors.CanNotBeEmpty, "title")
                    });
            }

            if (!String.IsNullOrEmpty(pressRelease.Title) && pressRelease.Title.Length > TitleLength)
            {
                errors.Add(
                    new Error
                    {
                        Message = String.Format(Errors.CanNotBeGreaterThan, "title", TitleLength)
                    });
            }

            if (pressRelease.DatePublished < DateTime.Now.AddMinutes(-5))
            {
                errors.Add(
                new Error
                {
                    Message = String.Format(Errors.DateMustBeInTheFuture, "publication date")
                });
            }
            return errors;
        }
    }
}
