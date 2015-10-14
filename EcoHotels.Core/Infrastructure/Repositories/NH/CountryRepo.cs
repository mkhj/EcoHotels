using System.Collections.Generic;
using EcoHotels.Core.Domain.Models;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    public class CountryRepo : Repository<Country>
    {
        public Country FindBy(int id)
        {
            return Session.Get<Country>(id);
        }

        public Country FindByISOCode(string code)
        {
            var criteria = DetachedCriteria.For(typeof(Country))
                .SetCacheable(true)
                .Add(Restrictions.Eq("Alpha2Code", code));

            return FindOne(criteria);
        }

        //public new IEnumerable<Country> FindAll()
        //{
        //    return FindAll();
        //}
    }
}
