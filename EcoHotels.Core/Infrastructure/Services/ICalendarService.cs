using System.Collections.Generic;
using EcoHotels.Core.Domain.Models;

namespace EcoHotels.Core.Infrastructure.Services
{
    public interface ICalendarService
    {
        IEnumerable<Date> FindAllDaysInMonthBy(int year, int month);
    }
}
