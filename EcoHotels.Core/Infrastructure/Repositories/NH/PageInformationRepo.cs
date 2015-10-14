using EcoHotels.Core.Domain.Models.Localization;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    internal class PageInformationRepo : Repository<PageInformation>
    {
        public PageInformation FindByFilename(string filename)
        {
            var criteria = DetachedCriteria.For(typeof (PageInformation))
                .Add(Restrictions.Eq("Filename", filename.Trim()));

            return FindOne(criteria);
        }

        public bool IsFilenameUnique(string filename)
        {
            var criteria = DetachedCriteria.For(typeof(PageInformation))
                .Add(Restrictions.Eq("Filename", filename.Trim()));

            return !Exists(criteria);
        }
    }
}
