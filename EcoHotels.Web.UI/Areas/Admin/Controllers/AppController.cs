using System;
using System.Web.Mvc;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    public class AppController : Controller
    {
        [HttpPost]
        public ActionResult Ping()
        {
            HttpContext.Session["KeepSessionAlive"] = DateTime.Now;

            return Json("");
        }

    }
}
