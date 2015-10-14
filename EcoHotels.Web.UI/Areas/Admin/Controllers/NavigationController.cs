using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models.Property;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class NavigationController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IOrganizationService OrganizationService { get; set; }

        [Dependency]
        public IHotelService HotelService { get; set; }

        [Dependency]
        public IRoomTypeService RoomTypeService { get; set; }

        [Dependency]
        public IAssetCategoryService AssetCategoryService { get; set; }

        [Dependency]
        public IRateService RateService { get; set; }

        [Dependency]
        public IAddonService AddonService { get; set; }

        #endregion

        [ChildActionOnly]
        public ActionResult MainNavigation(string page)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);
            if(organization.IsNull())
            {
                return View(new MainMenuModel(page, 0, new List<Hotel>()));
            }

            var hotelId = AppService.GetCurrentHotelId();            
            if(hotelId == 0)
            {
                var firstOrDefault = organization.Hotels.FirstOrDefault();
                hotelId = (firstOrDefault.IsNotNull()) ? firstOrDefault.Id : 0;

                AppService.SetCurrentHotelId(hotelId);
            }

            return View(new MainMenuModel(page, hotelId, organization.Hotels));
        }

        /// <summary>
        /// Dashboard section navigation.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Dashboard(string page)
        {
            return View(new DashboardSectionModel(page));
        }

        /// <summary>
        /// Dashboard section navigation.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Media(string page, int? folderId)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var categories = AssetCategoryService.FindAllByHotelId(currentOrganizationId);

            var selectedRoomtypeId = folderId ?? 0;

            return View(new MediaSectionModel(selectedRoomtypeId, categories, page));
        }

        /// <summary>
        /// Settings section navigation.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Settings(string page, int? id)
        {
            var selectedUserId = id ?? 0;

            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var organization = OrganizationService.FindById(currentOrganizationId);

            return View(new SettingSectionModel(selectedUserId, organization.Users, page));
        }

        /// <summary>
        /// Hotel section navigation.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="hotelId"></param>
        /// <param name="roomtypeId"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult Hotels(string page, int? roomtypeId)
        {
            var selectedRoomtypeId = roomtypeId ?? 0;

            var currentHotelId = AppService.GetCurrentHotelId();            
            var hotel = HotelService.FindById(currentHotelId);

            if(hotel.IsNull())
            {
                throw new ArgumentException("Hotel can not be null.");
            }

            return View(new HotelSectionModel(hotel.Id, selectedRoomtypeId, hotel.RoomTypes, page)); 
        }

        [ChildActionOnly]
        public ActionResult Rates(string page, int? id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var rateCategories = RateService.FindByHotel(currentHotelId);

            var selectedRateCategoryId = id ?? 0;

            return View(new RateSectionModel(selectedRateCategoryId, rateCategories, page));
        }

        [ChildActionOnly]
        public ActionResult Addons(string page, int? id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var addons = AddonService.FindAllByHotelId(currentHotelId);

            var selectedRateCategoryId = id ?? 0;

            return View(new AddonSectionModel(selectedRateCategoryId, addons, page));
        }
    }
}
