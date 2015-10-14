using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models.Price;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    //[AuthorizeAdmin]
    public class PricingController : Controller
    {

        #region - services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IRateService RateService { get; set; }

        [Dependency]
        public IInventoryService IInventoryService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public ICalendarService CalendarService { get; set; }

        #endregion



        [HttpGet]
        public ActionResult Index()
        {
            var currentHotelId = 1; // AppService.GetCurrentHotelId();
            var rateCategories = RateService.FindByHotel(currentHotelId);

            var hotel = HotelService.FindById(currentHotelId);
            var roomtypeId = hotel.RoomTypes.First().Id;

            var currentdate = DateTime.Now.Date;

            var roomTypeInventories = IInventoryService.FindByIds(new [] { roomtypeId }, currentdate.Year, currentdate.Month);

            return View(new PricingModel(rateCategories, roomTypeInventories, currentdate.Year, currentdate.Month));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(PricingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = 1; // AppService.GetCurrentHotelId();

            var hotel = HotelService.FindById(currentHotelId);
            var roomtypeIds = hotel.RoomTypes.Select(x => x.Id);

            var rateCategories = RateService.FindByHotel(currentHotelId);

            var roomTypeInventories = IInventoryService.FindByIds(roomtypeIds, model.SelectedYear, model.SelectedMonth).ToList();

            //Ensure inventory for RoomTypes
            var newInventories = IInventoryService.EnsureInventories(hotel, roomTypeInventories, model.SelectedYear, model.SelectedMonth);

            roomTypeInventories.AddRange(newInventories);


            for(var i = 0; i < model.DateWithPrice.Count; i++)
            {
                var pricingItem = model.DateWithPrice[i];

                var inventories = roomTypeInventories.Where(x => x.Date.Value.Day == i + 1);

                if (rateCategories.Any(x => x.Id == pricingItem.Id))
                {
                    inventories.ForEach(x => x.RateCategoryId = pricingItem.Id);  
                }
                else
                {
                    // it does exist which mean Id is Zero so we are going to use Rackrate instead by setting it to null
                    inventories.ForEach(x => x.RateCategoryId = null);
                }
                
            }

            IInventoryService.Save(roomTypeInventories);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        [HttpGet]
        public ActionResult Rates()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Rates(object model)
        {
            return View();
        }
    }
}
