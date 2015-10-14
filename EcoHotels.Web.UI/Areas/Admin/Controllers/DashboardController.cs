using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Web.Core.Attributes;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class DashboardController : Controller
    {

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IReservationService ReservationService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            var currentHotelId = AppService.GetCurrentHotelId();

            var endDate = DateTime.Now.AddDays(15);
            var startDate = endDate.AddDays(-15);

            var reservationsForStats = ReservationService.FindBy(currentHotelId, startDate, endDate);

            var upcomingGuests = ReservationService.FindBy(currentHotelId, startDate, startDate.AddDays(7));

            return View(new DashboardModel(reservationsForStats, upcomingGuests));
        }

    }
}
