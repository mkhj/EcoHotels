using System;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.UI.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    [AuthorizeCustomer]
    public class ReservationController : Controller
    {
        #region - Service -

        [Dependency]
        public IReservationService ReservationService { get; set; }

        [Dependency]
        public ICustomerService CustomerService { get; set; }

        #endregion

        [HttpGet]
        public ActionResult Index(int id)
        {
            var reservation = ReservationService.FindBy(User.Identity.Name.ToInt(), id);
            if(reservation.IsNull())
            {
                throw new ApplicationException("Reservation does not exist.");
            }

            return View(new ReservationModel(reservation));
        }

        [HttpGet]
        public ActionResult List()
        {
            var reservations = ReservationService.FindByCustomer(User.Identity.Name.ToInt());
            return View(new ReservationListModel(reservations));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            var reservation = ReservationService.FindBy(User.Identity.Name.ToInt(), id);
            if(reservation.IsNull())
            {
                return Json(new JsonResultError("Invalid reservation."));
            }

            reservation.Cancelled = DateTime.Now;
            ReservationService.Save(reservation);

            //TODO: send emails out
            var customer = CustomerService.FindById(User.Identity.Name.ToInt());


            return Json(new JsonResultSuccess("Reservation has been canceled."));
        }

    }
}
