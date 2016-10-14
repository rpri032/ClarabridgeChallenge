using ClarabridgeChallenge.Repository.Interfaces;
using System;

namespace ClarabridgeChallenge.Business.Assemblers
{
    public class PressReleaseAssembler
    {
        private static IPressReleaseRepository _pressReleaseRepository;
        private static IPressReleaseRepository PressReleaseRepository
        {
            get
            {
                return _pressReleaseRepository = (_pressReleaseRepository ?? UnityDependencyFactory.ResolveUnityContainer<IPressReleaseRepository>());
            }
        }

        internal static Models.PressRelease BuildRow(string title, string descriptionHtml,
            DateTime datePublished)
        {
            return BuildRow(Guid.Empty, title, descriptionHtml, datePublished);
        }

        internal static Models.PressRelease BuildRow(Guid id, string title, string descriptionHtml,
            DateTime datePublished)
        {
            Models.PressRelease pressRelease;
            if (id == Guid.Empty)
            {
                pressRelease = new Models.PressRelease
                {
                    Id = Guid.NewGuid()
                };
            }
            else
            {
                pressRelease = (Models.PressRelease)PressReleaseRepository.Get(id);
            }
            if (pressRelease != null)
            {
                pressRelease.Title = title;
                pressRelease.DescriptionHtml = descriptionHtml;
                pressRelease.DatePublished = datePublished;
                pressRelease.DateCreated = (id == Guid.Empty ? DateTime.Now : pressRelease.DateCreated);
                pressRelease.DateUpdated = (id == Guid.Empty ? pressRelease.DateUpdated : DateTime.Now);
            }
            return pressRelease;
        }
    }
}
