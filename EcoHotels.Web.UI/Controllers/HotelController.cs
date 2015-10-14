using System;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Value_objects;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    public class HotelController : Controller
    {
        #region - Services -

        [Dependency]
        public ICartService CartService { get; set; }

        [Dependency]
        public ICurrencyService CurrencyService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public ISearchService SearchService { get; set; }

        #endregion
        
        [HttpGet]
        public ActionResult Index(int id, DateTime? arrival, DateTime? departure)
        {
            SearchResultHotel searchResult;

            if(arrival.HasValue && departure.HasValue)
            {
                searchResult = SearchService.FindByHotel(1, arrival.Value, departure.Value);
            }
            else
            {
                searchResult = SearchService.FindByHotel(1);
            }

            Session.Add("ecohotels.searchresult", searchResult);

            return View(searchResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CartModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(new JsonResultError("Please fill out all the fields."));
            //}

            var searchResult = Session["ecohotels.searchresult"] as SearchResultHotel;
            if (searchResult.IsNull())
            {
                return Content("No Search Result..");
            }

            //TODO: Ensure roomtype Ids exist within search result

            var reservation = Reservation.Create(
                searchResult.Arrival,
                searchResult.Departure,
                "DKK",
                ReservationType.Person,
                Request.UserHostAddress
            );

            reservation.HotelId = searchResult.HotelId;
            reservation.HotelName = searchResult.Name;
            reservation.HotelPhoneNumber = searchResult.PhoneNumber;
            reservation.HotelEmail = searchResult.Email;
            

            var selectedRooms = model.Items.Where(x => x.Quantity > 0);
            foreach (var selectedRoom in selectedRooms)
            {
                var roomType = searchResult.Rooms.FirstOrDefault(x => x.RoomTypeId == selectedRoom.RoomTypeId);
                if(roomType.IsAvailable)
                {
                    for (var q = 0; q < selectedRoom.Quantity; q++)
                    {
                        var reservationItem = ReservationItem.Create(reservation, roomType.Name, ReservationItemType.Room);
                        reservationItem.RoomTypeId = roomType.RoomTypeId;

                        for (var i = 0; i < reservation.Duration; i++)
                        {
                            var searchResultRoomPrice = roomType.Prices.FirstOrDefault(x => x.Date == reservation.Arrival.Date.AddDays(i));

                            var reservationPrice = ReservationPrice.Create(reservationItem, reservation.Arrival.Date.AddDays(i), searchResultRoomPrice.Price);
                            reservationItem.PricePerDate.Add(reservationPrice);
                        }

                        reservation.Items.Add(reservationItem);
                    }
                }
            }


            CartService.Update(reservation);

            return RedirectToAction("index", "checkout");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Search(int hotelId, DateTime? arrival, DateTime? departure)
        {
            if (arrival.HasValue && departure.HasValue)
            {
                var searchResult = SearchService.FindByHotel(1, arrival.Value, departure.Value);
                
                Session.Add("ecohotels.searchresult", searchResult);

                return View(searchResult);
            }

            return Content("Dates not valid.");
        }

    }
}
