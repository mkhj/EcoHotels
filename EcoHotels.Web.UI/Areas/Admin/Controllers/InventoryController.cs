using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class InventoryController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public IInventoryService IInventoryService { get; set; }

        [Dependency]
        public IReservationService ReservationService { get; set; }

        [Dependency]
        public ICalendarService CalendarService { get; set; }

        #endregion


        [HttpGet]
        public ActionResult Index()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var currentdate = DateTime.Now.Date;

            var roomTypeInventories = IInventoryService.FindByIds(hotel.RoomTypes.Select(x => x.Id), currentdate.Year, currentdate.Month);

            var startDate = new DateTime(currentdate.Year, currentdate.Month, 1);
            var endDate = new DateTime(currentdate.Year, currentdate.Month, DateTime.DaysInMonth(currentdate.Year, currentdate.Month));

            var reservations = ReservationService.FindBy(currentHotelId, startDate, endDate);

            return View(new InventoryModel(hotel, roomTypeInventories, reservations, currentdate.Month, currentdate.Year));            
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(InventoryModel model)
        {
            #region - Defensive programming -

            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Inventory model is not valid."));
            }

            var currentHotelId = 1; // AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);
            if(hotel.IsNull())
            {
                return Json(new JsonResultError("Hotel can not be null."));
            }


            var roomTypes = hotel.RoomTypes;

            if(!roomTypes.All(x => model.Rooms.Exists(y => x.Id == y.RoomTypeId)))
            {
                return Json(new JsonResultError("Inventory Roomtype is not valid."));
            }

            #endregion

            var month = CalendarService.FindAllDaysInMonthBy(model.SelectedYear, model.SelectedMonth);

            var startDate = new DateTime(model.SelectedYear, model.SelectedMonth, 1);
            var endDate = new DateTime(model.SelectedYear, model.SelectedMonth, DateTime.DaysInMonth(model.SelectedYear, model.SelectedMonth));
            var reservations = ReservationService.FindBy(currentHotelId, startDate, endDate);
            var reservationItems = reservations.SelectMany(x => x.Items);
            
            var newInventories = new List<RoomTypeInventory>();

            // Find all available inventories from the roomtypes
            var currentInventories = IInventoryService.FindByIds(roomTypes.Select(x => x.Id), model.SelectedYear, model.SelectedMonth).ToList();
            foreach (var inventory in model.Rooms)
            {

                var reservationsItemsForRoomtype = reservationItems.Where(x => x.RoomTypeId == inventory.RoomTypeId);

                var roomtype = roomTypes.First(x => x.Id == inventory.RoomTypeId);

                foreach (var newAvailableQuantity in inventory.Quantities)
                {
                    var numberOfReservations = reservationsItemsForRoomtype.SelectMany(x => x.PricePerDate).Count(x => x.Date == newAvailableQuantity.Date);

                    // Goes through all quantaties and dates adding those that are missing
                    var roomTypeInventory = currentInventories.FirstOrDefault(x => x.RoomTypeId == inventory.RoomTypeId && x.Date.Value == newAvailableQuantity.Date);
                    if(roomTypeInventory.IsNull())
                    {
                        var date = month.First(x => x.Value == newAvailableQuantity.Date.Date);

                        var newInventory = RoomTypeInventory.Create(roomtype.Id, date);

                        newInventory.Quantity = newAvailableQuantity.Value + numberOfReservations;

                        newInventories.Add(newInventory);
                    }
                    else
                    {
                        roomTypeInventory.Quantity = newAvailableQuantity.Value + numberOfReservations;
                    }
                }
            }   

            currentInventories.AddRange(newInventories);

            IInventoryService.Save(currentInventories);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult List(int selectedYear, int selectedMonth)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);
            if(hotel.IsNull())
            {
                return Content("Hotel is not valid.");
            }

            var roomTypeInventories = IInventoryService.FindByIds(hotel.RoomTypes.Select(x => x.Id), selectedYear, selectedMonth);

            var startDate = new DateTime(selectedYear, selectedMonth, 1);
            var endDate = new DateTime(selectedYear, selectedMonth, DateTime.DaysInMonth(selectedYear, selectedMonth));

            var reservations = ReservationService.FindBy(currentHotelId, startDate, endDate);

            return View(new InventoryModel(hotel, roomTypeInventories, reservations, selectedMonth, selectedYear)); 
        }

    }
}
