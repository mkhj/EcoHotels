using System;
using System.Collections.Generic;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class LanguageService : ILanguageService
    {
        private ICacheStorage CacheStorage;

        private Repository<Language> LanguageRepo;

        public LanguageService(ICacheStorage cacheStorage)
        {
            LanguageRepo = new Repository<Language>();
            CacheStorage = cacheStorage;
        }

        public Language FindById(int id)
        {
            return LanguageRepo.FindBy(id);
        }

        public Language FindByShortName(string shortname)
        {
            var criteria = DetachedCriteria.For(typeof(Language))
                .Add(Restrictions.Eq("Shortname", shortname));

            return LanguageRepo.FindOne(criteria);
        }

        /// <summary>
        /// System Langauge is English
        /// </summary>
        /// <returns></returns>
        public Language FindSystemDefault()
        {
            return FindById((int)LanguageTypeEnum.English);
        }

        public IEnumerable<Language> FindByOrganizationId(Guid id)
        {
            var criteria = DetachedCriteria.For(typeof(Organization))
                .Add(Restrictions.Eq("Id", id))
                .CreateAlias("AvailableLanguages", "a")
                .SetProjection(Projections.ProjectionList()
                    .Add(Projections.Property("a.Id"), "Id")
                    .Add(Projections.Property("a.Name"), "Name")
                )
                .SetResultTransformer(Transformers.AliasToBean(typeof(Language)));

            return LanguageRepo.FindAll(criteria);
        }

        public IEnumerable<Language> FindAll()
        {
            return LanguageRepo.FindAll();
        }

        public IEnumerable<Language> FindAllActive()
        {
            var criteria = DetachedCriteria.For(typeof(Language))
                .Add(Restrictions.Eq("IsActive", true));

            return LanguageRepo.FindAll(criteria);
        }

        public void Save(Language language)
        {
            if (language.IsValid())
            {
                LanguageRepo.Save(language);
            }
        }
    }
}
