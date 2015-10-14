using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Email;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    [Localized]
    public class CheckoutController : Controller
    {
        #region - Services -

        [Dependency]
        public IAddonService AddonService { get; set; }

        [Dependency]
        public ICartService CartService { get; set; }

        [Dependency]
        public ICountryService CountryService { get; set; }

        [Dependency]
        public ICurrencyService CurrencyService { get; set; }

        [Dependency]
        public IReservationService ReservationService { get; set; }

        [Dependency]
        public ICustomerService CustomerService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }        

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            // if we have no cart, something but be wrong.. turn back??
            var reservation = CartService.GetCartContent();
            if (reservation.IsNull())
            {
                return Content("No reservation in progress..");
            }

            var searchResult = Session["ecohotels.searchresult"] as SearchResultHotel;
            if (searchResult.IsNull())
            {
                return Content("No Search Result..");
            }

            var countries = CountryService.FindAll();

            var addons = AddonService.FindAllByHotelId(reservation.HotelId);

            var customer = CustomerService.FindById(User.Identity.Name.ToInt());
            if(customer.IsNotNull())
            {
                reservation.Firstname = customer.Firstname;
                reservation.Lastname = customer.Lastname;
                reservation.Email = customer.Email;
                reservation.PhoneNumber = customer.PhoneNumber;
                reservation.Country = customer.Country;
            }

            return View(new CheckoutBillingInformationModel(reservation, searchResult, addons, countries));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CheckoutBillingInformationModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new JsonResultError("Please fill out all the fields."));
            }

            var reservation = CartService.GetCartContent();
            if(reservation.IsNull())
            {
                return Json(new JsonResultError("Your cart has expired."));
            }

            reservation.Customer = CustomerService.FindById(User.Identity.Name.ToInt());

            reservation.Firstname = model.Firstname;
            reservation.Lastname = model.Lastname;
            reservation.Email = model.Email;
            reservation.PhoneNumber = model.PhoneNumber;
            reservation.Country = model.SelectedCountryName;

            var addons = AddonService.FindAllByHotelId(reservation.HotelId);

            for (var i = 0; i < model.Rooms.Count; i++)
            {
                var item = model.Rooms[i];
                var reservationItem = reservation.Items.ElementAt(i);

                reservationItem.NameOfPrimaryGuest = item.NameOfPrimaryGuest;
                reservationItem.Adults = item.Adults;
                reservationItem.Children = 0;
                reservationItem.Babies = 0;

                var selectedAddonIds = item.Addons
                                            .Where(x => x.IsSelected)
                                            .Select(x => x.Id);

                var selectedAddons = addons
                                        .Where(x => selectedAddonIds.Contains(x.Id))
                                        .Select(x => ReservationAddon.Create(reservationItem, x))
                                        .ToList();

                reservationItem.SetAddons(selectedAddons);
            }


            ReservationService.Save(reservation);

            var hotel = HotelService.FindById(reservation.HotelId);

            // Send confirmation email
            new EmailService().SendReservationEmailToCustomer(hotel, reservation);

            // transfer cart to next step??
            TempData["Reservation"] = reservation;
            TempData.Keep("Reservation");

            // Clear Cart
            //CartService.Remove();

            return Json(new JsonResultSuccess("Done.", new { created = true}));
        }


        [HttpGet]
        public ActionResult Complete()
        {
            // should not go here if no reservation has been made
            //var reservation = TempData["Reservation"] as Reservation;
            //if(reservation.IsNull())
            //{
            //    return View("error");
            //}

            // in View show confirmation

            var reservation = CartService.GetCartContent();

            return View(new CheckoutCompleteModel(reservation));
        }

    }
}
