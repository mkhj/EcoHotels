using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Media;

namespace EcoHotels.Web.UI.Areas.Admin.Models
{
    public class DashboardModel
    {
        public DashboardModel()
        {
            TotalNumberOfRoomsBookedInMonth = new Dictionary<object, object> {{"Days", "Rooms"}};
            UpcomingGuest = new List<ReservationStatsModel>();
        }

        public DashboardModel(IEnumerable<Reservation> reservations, IEnumerable<Reservation> upcomingGuests) : this()
        {
            var startDate = DateTime.Now.AddDays(-15);

            for (var i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i);
                var numberOfRoomsBooked = reservations.Where(x => x.Arrival <= date && date < x.Departure).SelectMany(x => x.Items).Count();
                TotalNumberOfRoomsBookedInMonth.Add(date.Day, numberOfRoomsBooked);
            }

            UpcomingGuest = upcomingGuests.Select(x => new ReservationStatsModel { ReservationId = x.Id, Fullname = x.Fullname, Arrival = x.Arrival });
        }

        public Dictionary<object, object> TotalNumberOfRoomsBookedInMonth { get; set; }

        public IEnumerable<ReservationStatsModel> UpcomingGuest { get; set; }
    }

    public class ReservationStatsModel
    {

        public int ReservationId { get; set; }

        public string Fullname { get; set; }

        public DateTime Arrival { get; set; }
    }
}