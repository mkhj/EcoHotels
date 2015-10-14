using System.Collections.Generic;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Infrastructure.Cache;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class CountryService : ICountryService
    {
        private Repository<Country> CountryRepo;
        private ICacheStorage CacheStorage;

        public CountryService(ICacheStorage cacheStorage)
        {            
            CacheStorage = cacheStorage;

            CountryRepo = new Repository<Country>();
        }

        public Country FindByISOCode(string code)
        {
            var criteria = DetachedCriteria.For(typeof(Country))
                .SetCacheable(true)
                .Add(Restrictions.Eq("Alpha2Code", code));

            return CountryRepo.FindOne(criteria);
        }

        public IEnumerable<Country> FindAll()
        {
            return CountryRepo.FindAll();
        }
    }
}
