using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Infrastructure.NH;
using EcoHotels.Core.Infrastructure.Repositories.NH;
using EcoHotels.Extensions;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services
{
    public class PageInformationService : IPageInformationService
    {
        private Repository<PageInformation> PageInformationRepo;

        public PageInformationService()
        {
            PageInformationRepo = new Repository<PageInformation>();
        }

        public PageInformation FindByFilename(string filename)
        {
            var criteria = DetachedCriteria.For(typeof(PageInformation))
                .Add(Restrictions.Eq("Filename", filename.Trim()));

            return PageInformationRepo.FindOne(criteria);
        }

        public IEnumerable<PageInformation> FindAll()
        {
            return PageInformationRepo.FindAll();
        }

        public bool IsFilenameUnique(string filename)
        {
            filename = filename.GenerateSlug();

            var criteria = DetachedCriteria.For(typeof(PageInformation))
                .Add(Restrictions.Eq("Filename", filename.Trim()));

            return !PageInformationRepo.Exists(criteria);
        }
    }
}
