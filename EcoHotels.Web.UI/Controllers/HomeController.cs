using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using EcoHotels.Core.Infrastructure.Repositories.NH.Tags;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Web.Core.Attributes;
using EcoHotels.Web.Core.Marketing;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    [ContentExperiment, SessionState(SessionStateBehavior.Disabled)]
    public class HomeController : Controller
    {

        //[HttpGet, OutputCache(CacheProfile = "BasicOutputCache")]
        [HttpGet]
        public ActionResult Index()
        {
            var tags = new SearchTagCityRepo().FindAll();

            return View(new HomepageModel(tags));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(SearchModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new JsonResultError("Please fill out all the fields."));
            }

            return RedirectToAction("index", "search", 
                    new
                        {
                            city = model.CityId,
                            arrival = model.Arrival.Date.ToString("MM-dd-yyyy"),
                            departure = model.Departure.Date.ToString("MM-dd-yyyy")
                        }
                );
        }
    }
}
