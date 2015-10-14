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
using EcoHotels.Web.UI.Areas.Admin.Models.Price;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class RateController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public IRoomTypeService RoomTypeService { get; set; }

        [Dependency]
        public IRateService RateService { get; set; }

        #endregion

        /// <summary>
        /// Rack Rates.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var roomTypes = RoomTypeService.FindAllByHotelId(currentHotelId);

            return View(new RackRateModel(roomTypes));
        }

        /// <summary>
        /// Rack rates.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(RackRateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            foreach (var rackRateItemModel in model.Items)
            {
                var roomType = hotel.RoomTypes.FirstOrDefault(x => x.Id == rackRateItemModel.RoomTypeId);
                if(roomType.IsNotNull())
                {
                    roomType.RackRate = rackRateItemModel.Value;
                }
            }


            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var roomTypes = RoomTypeService.FindAllByHotelId(currentHotelId);

            return View(new CreateRateCategoryModel(roomTypes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CreateRateCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var rateCategory = RateCategory.Create(hotel, model.Name, model.Description);
            rateCategory.Color = model.Color;

            foreach (var itemModel in model.Items)
            {
                var roomType = hotel.RoomTypes.FirstOrDefault(x => x.Id == itemModel.RoomTypeId);
                if (roomType.IsNotNull())
                {
                    var ratePrice = RatePrice.Create(rateCategory, itemModel.RoomTypeId, itemModel.Value, itemModel.IsActive);
                    rateCategory.Items.Add(ratePrice);
                }
            }

            RateService.Save(rateCategory);

            var url = string.Format("/admin/rate/edit/{0}", rateCategory.Id);

            return Json(new JsonResultSuccess("Created succesfully.", new { url = url, name = rateCategory.Name, created = true }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var roomTypes = RoomTypeService.FindAllByHotelId(currentHotelId);

            var rateCategory = RateService.FindBy(id, currentHotelId);

            return View(new EditRateCategoryModel(rateCategory, roomTypes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditRateCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var rateCategory = RateService.FindBy(model.Id, currentHotelId);

            if(rateCategory.IsNull())
            {
                return Json(new JsonResultError("Rate category can not be null."));
            }

            rateCategory.Name = model.Name;
            rateCategory.Description = model.Description;
            rateCategory.Color = model.Color;

            foreach (var itemModel in model.Items)
            {
                var ratePrice = rateCategory.Items.FirstOrDefault(x => x.RoomTypeId == itemModel.RoomTypeId);
                if (ratePrice.IsNotNull())
                {
                    ratePrice.Value = itemModel.Value;
                    ratePrice.IsActive = itemModel.IsActive;
                }
            }

            RateService.Save(rateCategory);
            
            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var rateCategory = RateService.FindBy(id, hotel.Id);
            if (rateCategory.IsNull())
            {
                return Json(new JsonResultError("Rate category can not be null."));
            }

            RateService.Delete(rateCategory);

            return Json(new JsonResultSuccess("Deleted succesfully."));
        }
    }
}
