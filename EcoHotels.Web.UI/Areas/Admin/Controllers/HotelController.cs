using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Helpers;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models.Property;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class HotelController : Controller
    {
        #region - Services -

        [Dependency]
        public IAmenityService AmenityService { get; set; }

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public ILanguageService LanguageService { get; set; }
        
        [Dependency]
        public IOrganizationService OrganizationService { get; set; }

        [Dependency]
        public ICountryService CountryService { get; set; }

        [Dependency]
        public ICurrencyService CurrencyService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public IAssetCategoryService AssetCategoryService { get; set; }

        [Dependency]
        public IAssetService AssetService { get; set; }

        #endregion

        [HttpGet]
        public ActionResult Switch(int id)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var hotel = HotelService.FindById(currentOrganizationId, id);

            if(hotel.IsNull())
            {
                throw new ArgumentNullException("Hotel can not be null");
            }

            AppService.SetCurrentHotelId(hotel.Id);

            return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            var countries = CountryService.FindAll();
            var currencies = CurrencyService.FindAll();

            return View(new CreateHotelModel(countries, currencies));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CreateHotelModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new JsonResultError("Hotel model is not valid."));
            }


            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);
            var currency = CurrencyService.FindById(model.SelectedCurrencyId);

            var hotel = Hotel.Create(model.Name, model.VatNo, organization);
            hotel.Address1 = model.Address1;
            hotel.Address2 = model.Address2;
            hotel.City = model.City;
            hotel.Zipcode = model.Zipcode;
            hotel.Country = model.SelectedCountryName;
            hotel.Fax = model.Fax;
            hotel.Email = model.Email;
            hotel.WWW = model.WWW;

            hotel.Currency = currency;

            organization.Hotels.Add(hotel);

            OrganizationService.Save(organization);

            return Json(new JsonResultSuccess("Created succesfully.", new { id = hotel.Id, created = true }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ApplicationException("Hotel does not exist.");
            }

            var countries = CountryService.FindAll();

            return View(new EditHotelInfoModel(hotel, countries));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(EditHotelInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            hotel.Address1 = model.Address1;
            hotel.Address2 = model.Address2;
            hotel.City = model.City;
            hotel.Country = model.SelectedCountryName;
            hotel.Email = model.Email;
            hotel.Fax = model.Fax;
            hotel.Name = model.Name;
            hotel.Phone = model.Phone;
            hotel.VatNo = model.VatNo;
            hotel.WWW = model.WWW;
            hotel.Zipcode = model.Zipcode;
            hotel.Modified = DateTime.Now;

            HotelService.Save(hotel);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var hotel = HotelService.FindById(currentOrganizationId, id);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            HotelService.Delete(hotel);

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Photos()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            var assets = AssetCategoryService.FindAllByHotelId(AppService.GetCurrentOrganizationId())
                            .SelectMany(x => x.Items);

            return View(new EditHotelPhotoModel(hotel.Assets, assets));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Photos(EditHotelPhotoModel model)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            hotel.Assets.Clear();

            if (model.Assets.IsNotNull())
            {
                var ids = model.Assets.Select(x => x.Id).Distinct();
                var assets = AssetService.FindByIds(AppService.GetCurrentOrganizationId(), ids);

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

                    hotel.Assets.Add(asset);
                }
            }

            hotel.Modified = DateTime.Now;
            
            HotelService.Save(hotel);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Policies()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ApplicationException("Hotel does not exist.");
            }


            var languages = new List<Language> { LanguageService.FindById((int) LanguageTypeEnum.English) };

            return View(new EditHotelPolicyModel(hotel, languages));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Policies(EditHotelPolicyModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ApplicationException("Hotel does not exist.");
            }
            
            var language = LanguageService.FindById((int) LanguageTypeEnum.English);

            hotel.PaymentPolicy.AddLocalizedText(model.Payment[0].Value, language);
            hotel.CancellationPolicy.AddLocalizedText(model.Cancellation[0].Value, language);

            hotel.Descriptions.Clear();

            HotelService.Save(hotel);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ApplicationException("Hotel does not exist.");
            }

            var languages = LanguageService.FindAllActive(); // new List<Language> { .FindById((int) LanguageTypeEnum.English) };
            var amenities = AmenityService.FindAll();

            return View(new EditHotelDescriptionModel(hotel, amenities, languages));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Details(EditHotelDescriptionModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data not valid"));
            }

            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            if (hotel.IsNull())
            {
                throw new ApplicationException("Hotel does not exist.");
            }

            hotel.CheckIn = model.CheckIn;
            hotel.CheckInAMPM = model.CheckInAMPM;
            hotel.CheckOut = model.CheckOut;
            hotel.CheckOutAMPM = model.CheckOutAMPM;

            hotel.MinimumStay = model.MinimumStay;
            hotel.MinimumCheckInAge = model.MinimumCheckInAge;

            hotel.CategoryOne = model.SelectedCategoryOne.ConvertToEnum<HotelCategoryEnum>();
            hotel.CategoryTwo = model.SelectedCategoryTwo.ConvertToEnum<HotelCategoryEnum>();
            hotel.CategoryThree = model.SelectedCategoryThree.ConvertToEnum<HotelCategoryEnum>();

            #region - Update Descriptions -

            var languages = LanguageService.FindAllActive();

            LocalizedValueHelper.MapToMultiLanguageText(model.About, hotel.About, languages);

            //foreach (var modelBulletPoint in model.WhatWeLove)
            //{
            //    var point = hotel.Descriptions.FirstOrDefault(x => x.Id == modelBulletPoint.Id);
            //    if(point.IsNotNull())
            //    {
            //        LocalizedValueHelper.MapToMultiLanguageText(modelBulletPoint.Text, point.Text, languages);    
            //    }                
            //}

            //foreach (var modelBulletPoint in model.WhatYouNeedToKnow)
            //{
            //    var point = hotel.Descriptions.FirstOrDefault(x => x.Id == modelBulletPoint.Id);
            //    if (point.IsNotNull())
            //    {
            //        LocalizedValueHelper.MapToMultiLanguageText(modelBulletPoint.Text, point.Text, languages);
            //    }
            //}

            #endregion

            #region - Amenities -

            var amenities = AmenityService.FindAll();
            hotel.Amenities.Clear();

            foreach (var amenity in model.Amenities)
            {
                if(amenity.IsSelected)
                {
                    var selectedAmenity = amenities.FirstOrDefault(x => x.Id == amenity.Id);
                    if(selectedAmenity.IsNotNull())
                    {
                        hotel.Amenities.Add(selectedAmenity);
                    }
                }
            }

            #endregion




            HotelService.Save(hotel);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }
    }
}
