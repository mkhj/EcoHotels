using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Web.Core.Attributes;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    [Localized]
    public class SearchController : Controller
    {
        #region - Services -

        [Dependency]
        public ISearchService SearchService { get; set; }

        #endregion

        [HttpGet]
        public ActionResult Index(int city, DateTime arrival, DateTime departure)
        {
            var searchResultList = SearchService.FindByCity(city, arrival, departure);

            return View(searchResultList);
        }
    }
}
