using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Infrastructure.NH;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.Services.Impl
{
    public class CalendarService : ICalendarService
    {
        private Repository<Date> DateRepo;

        public CalendarService()
        {
            DateRepo = new Repository<Date>();
        }

        public IEnumerable<Date> FindAllDaysInMonthBy(int year, int month)
        {
            var firstDateInMonth = new DateTime(year, month, 1);
            var lastDateInMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var criteria = DetachedCriteria.For(typeof(Date))
                    .Add(Restrictions.Ge("Value", firstDateInMonth.Date))
                    .Add(Restrictions.Le("Value", lastDateInMonth.Date));

            return DateRepo.FindAll(criteria);
        }
    }
}
