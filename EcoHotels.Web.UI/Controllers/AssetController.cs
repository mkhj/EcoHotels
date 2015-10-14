using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using EcoHotels.Core.Infrastructure.Services;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    public class AssetController : Controller
    {
        [Dependency]
        public IAssetService AssetService { get; set; }

        public FileContentResult Get(int id, string filename)
        {
            var asset = AssetService.FindById(id);
            //return Content(asset.Data.Length.ToString());   

            // writes directly to the stream
            var webImage = new WebImage(asset.Data)
                //.Resize(100, 100, true, true)
                .Write(asset.MimeType)
                .GetBytes(asset.MimeType);

            return File(new byte[0], asset.MimeType);
        }

    }
}
