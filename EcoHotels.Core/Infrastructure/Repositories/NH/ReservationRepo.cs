using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models.Commerce;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Repositories.NH
{
    public class ReservationRepo : Repository<Reservation>
    {
        public IEnumerable<Reservation> FindByDates(DateTime arrival, DateTime departure)
        {
            // Le - this_.Arrival >= @p0 
            // Ge - this_.Departure <= @p1

            var restriction1 = new Conjunction()
                .Add(Restrictions.Le("Arrival", arrival.Date))
                .Add(Restrictions.Ge("Departure", arrival.Date));

            var restriction2 = new Conjunction()
                .Add(Restrictions.Le("Arrival", departure.Date))
                .Add(Restrictions.Ge("Departure", departure.Date));

            var restriction3 = new Conjunction()
                .Add(Restrictions.Ge("Arrival", arrival.Date))
                .Add(Restrictions.Le("Departure", departure.Date));

            var query = new Disjunction().Add(restriction1).Add(restriction2).Add(restriction3);

            var criteria = DetachedCriteria.For(typeof(Reservation))
                .Add(query);

            return FindAll(criteria);
        }
    }
}
