using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Common.Enum;
using EcoHotels.Core.Domain.Models.Commerce;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models.Price;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class AddonController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IAddonService AddonService { get; set; }

        [Dependency]
        public ILanguageService LanguageService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("create");
        }


        [HttpGet]
        public ActionResult Create()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var languages = new List<Language> { LanguageService.FindById((int)LanguageTypeEnum.English) };

            return View(new AddonModel(hotel.Addons, languages, LanguageTypeEnum.English, hotel.Currency.ISOCurrencySymbol));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(AddonModel model)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var languages = new List<Language> { LanguageService.FindById((int)LanguageTypeEnum.English) };

            var addon = Addon.Create(hotel, model.Price);

            for (var i = 0; i < languages.Count(); i++)
            {
                addon.Name.AddLocalizedText(model.Name[i].Value, languages[i]);
                addon.Description.AddLocalizedText(model.Description[i].Value, languages[i]);
            }

            addon.CalculationRule = model.SelectedCalculationRule;
            addon.PostingRhythm = model.SelectedPostingRhythm;

            hotel.Addons.Add(addon);

            HotelService.Save(hotel);

            var url = string.Format("/admin/addon/edit/{0}", addon.Id);

            return Json(new JsonResultSuccess("Created succesfully.", new { url = url, name = addon.Name.GetText(LanguageTypeEnum.English), created = true }));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var languages = new List<Language> { LanguageService.FindById((int)LanguageTypeEnum.English) };

            if (!hotel.Addons.Any(x => x.Id == id))
            {
                throw new ApplicationException("Addon could not be found.");
            }

            return View(new AddonModel(id, hotel.Addons, languages, LanguageTypeEnum.English, hotel.Currency.ISOCurrencySymbol));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AddonModel model)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var addons = AddonService.FindAllByHotelId(currentHotelId);

            var languages = new List<Language> { LanguageService.FindById((int)LanguageTypeEnum.English) };

            var addon = addons.Where(x => x.Id == model.Id).FirstOrDefault();

            if (addon == null)
            {
                throw new ApplicationException("Addon could not be found.");
            }

            for (var i = 0; i < languages.Count(); i++)
            {
                addon.Name.AddLocalizedText(model.Name[i].Value, languages[i]);
                addon.Description.AddLocalizedText(model.Description[i].Value, languages[i]);
            }

            addon.Price = model.Price;
            addon.CalculationRule = model.SelectedCalculationRule;
            addon.PostingRhythm = model.SelectedPostingRhythm;

            AddonService.Save(addon);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var hotel = HotelService.FindById(currentHotelId);

            var addon = hotel.Addons.Where(x => x.Id == id).FirstOrDefault();

            if (addon == null)
            {
                return Json(new JsonResultError("Addon could not be deleted."));
            }

            hotel.Addons.Remove(addon);

            HotelService.Save(hotel);

            return Json(new JsonResultSuccess("Deleted succesfully."));
        }

    }
}
