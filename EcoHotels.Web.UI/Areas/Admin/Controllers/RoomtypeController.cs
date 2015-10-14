using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models.Property;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class RoomtypeController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public ILanguageService LanguageService { get; set; }

        [Dependency]
        public IAssetService AssetService { get; set; }

        [Dependency]
        public IAmenityService AmenityService { get; set; }

        [Dependency]
        public IAssetCategoryService AssetCategoryService { get; set; }

        [Dependency]
        public IRateService RateService { get; set; }

        #endregion

        [HttpGet]
        public ActionResult Create()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            var languages = new List<Language> {LanguageService.FindById((int) LanguageTypeEnum.English)};

            var assets = AssetCategoryService.FindAllByHotelId(AppService.GetCurrentOrganizationId())
                            .SelectMany(x => x.Items);

            return View(new CreateRoomTypelModel(languages, assets, hotel.Currency.ISOCurrencySymbol));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CreateRoomTypelModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Roomtype model is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            var language = LanguageService.FindById((int) LanguageTypeEnum.English);

            var roomType = RoomType.Create(hotel, "Unknown", language);

            #region - Update Properties -

            roomType.Name.AddLocalizedText(model.Name[0].Value, language);
            roomType.Description.AddLocalizedText(model.Description[0].Value, language);

            roomType.Capacity = model.Capacity;
            roomType.PhysicalRooms = model.PhysicalRooms;
            roomType.Size = model.Size;
            roomType.IsSmoking = model.Smoking;

            roomType.RackRate = model.RackRate;

            //if(model.Amenities.IsNotNull())
            //{
            //    var result = new Collection<Amenity>();
            //    foreach (var item in model.Amenities)
            //    {
            //        var amenity = hotel.Amenities.First(x => x.Id == item.Id);
            //        if (amenity != null && item.IsSelected)
            //        {
            //            result.Add(amenity);
            //        }
            //    }

            //    roomType.Amenities.Clear();
            //    roomType.Amenities = result;
            //}

            #endregion

            #region - Assets -

            if (model.Assets.IsNotNull())
            {
                var ids = model.Assets.Select(x => x.Id).Distinct();
                var assets = AssetService.FindByIds(currentHotelId, ids);

                foreach (var asset in assets)
                {
                    var selectedAsset = model.Assets.FirstOrDefault(x => x.Id == asset.Id);
                    if (selectedAsset.IsNotNull())
                    {
                        asset.SetCroppingInformation(
                             selectedAsset.TopX,
                             selectedAsset.TopY,
                             selectedAsset.BottomX,
                             selectedAsset.BottomY,
                             selectedAsset.CropXUnits,
                             selectedAsset.CropYUnits
                             );
                    }

                    roomType.Assets.Add(asset);
                }
            }

            #endregion

            hotel.RoomTypes.Add(roomType);
            HotelService.Save(hotel);

            RateService.EnsureRatesForNewRoomType(roomType);

            var url = string.Format("/admin/roomtype/edit/{0}", roomType.Id);

            return Json(new JsonResultSuccess("Created succesfully.", new { url = url, name = roomType.Name.GetText(LanguageTypeEnum.English), created = true }));
        }

        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            var roomType = hotel.RoomTypes.FirstOrDefault(x => x.Id == id);
            if (roomType.IsNull())
            {
                throw new ArgumentException("RoomType can not be null.");
            }

            var languages = new List<Language> { LanguageService.FindById((int)LanguageTypeEnum.English) };

            var assets = AssetCategoryService.FindAllByHotelId(currentHotelId)
                            .SelectMany(x => x.Items);
             
            return View(new EditRoomTypeModel(roomType, languages, assets, hotel.Currency.ISOCurrencySymbol));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditRoomTypeModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            var roomType = hotel.RoomTypes.FirstOrDefault(x => x.Id == model.RoomTypeId);
            if (roomType.IsNull())
            {
                throw new ArgumentException("RoomType can not be null.");
            }


            #region - Assets -

            roomType.Assets.Clear();

            if (model.Assets.IsNotNull())
            {
                var ids = model.Assets.Select(x => x.Id).Distinct();
                var assets = AssetService.FindByIds(currentHotelId, ids);

                foreach (var asset in assets)
                {
                    var selectedAsset = model.Assets.FirstOrDefault(x => x.Id == asset.Id);
                    if (selectedAsset.IsNotNull())
                    {
                        asset.SetCroppingInformation(
                             selectedAsset.TopX,
                             selectedAsset.TopY,
                             selectedAsset.BottomX,
                             selectedAsset.BottomY,
                             selectedAsset.CropXUnits,
                             selectedAsset.CropYUnits
                             );
                    }

                    roomType.Assets.Add(asset);
                }
            }

            #endregion

            #region - Update Properties -

            var languages = new List<Language> { LanguageService.FindById((int) LanguageTypeEnum.English) };

            for (var i = 0; i < languages.Count(); i++)
            {
                roomType.Name.AddLocalizedText(model.Name[i].Value, languages.ElementAt(i));
                roomType.Description.AddLocalizedText(model.Description[i].Value, languages.ElementAt(i));
            }

            roomType.Size = model.Size;
            roomType.Capacity = model.Capacity;
            roomType.PhysicalRooms = model.PhysicalRooms;
            roomType.IsSmoking = model.Smoking;
            roomType.RackRate = model.RackRate;

            #endregion

            HotelService.Save(hotel);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int roomTypeId)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var roomType = hotel.RoomTypes.FirstOrDefault(x => x.Id == roomTypeId);
            if (roomType == null)
            {
                return Json(new JsonResultError("Room Type could not be deleted."));
            }

            RateService.RemoveRatesForRoomType(roomType);

            hotel.RoomTypes.Remove(roomType);

            HotelService.Save(hotel);

            

            return Json(new JsonResultSuccess("Deleted succesfully."));
        }
    }
}
