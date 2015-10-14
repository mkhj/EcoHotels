using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using EcoHotels.Web.UI.Areas.Admin.Models.Property;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class SettingsController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IOrganizationService OrganizationService { get; set; }

        public IHotelService HotelService { get; set; }

        [Dependency]
        public ICountryService CountryService { get; set; }

        [Dependency]
        public ICurrencyService CurrencyService { get; set; }

        #endregion


        [HttpGet]
        public ActionResult General()
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);

            var countries = CountryService.FindAll();

            return View(new EditOrganizationModel(organization, countries));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult General(EditOrganizationModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data not valid."));
            }

            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);

            organization.Name = model.Name;
            organization.Address1 = model.Address1;
            organization.Address2 = model.Address2;
            organization.City = model.City;
            organization.Zipcode = model.Zipcode;
            organization.Phone = model.Phone;
            organization.Email = model.Email;
            organization.Country = model.SelectedCountryName;

            OrganizationService.Save(organization);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        [HttpGet]
        public ActionResult Currency()
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);
            var currencies = CurrencyService.FindAll();

            return View(new EditHotelCurrenciesModel(organization.Hotels, currencies));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Currency(EditHotelCurrenciesModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("Data not valid."));
            }

            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);
            var currencies = CurrencyService.FindAll();

            foreach (var item in model.Items)
            {
                var hotel = organization.Hotels.First(x => x.Id == item.HotelId);
                if(hotel.IsNotNull())
                {
                    var currency = currencies.First(x => x.Id == item.SelectedCurrencyId);
                    if(currency.IsNotNull())
                    {
                        hotel.Currency = currency;
                    }
                }
            }

            OrganizationService.Save(organization);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

    }
}
