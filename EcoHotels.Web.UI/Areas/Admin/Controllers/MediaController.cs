using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using EcoHotels.Core.Domain.Models;
using EcoHotels.Core.Domain.Models.Media;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using EcoHotels.Web.Core.Attributes.Security;
using EcoHotels.Web.Core.Models;
using EcoHotels.Web.Core.Services;
using EcoHotels.Web.UI.Areas.Admin.Models;
using EcoHotels.Web.UI.Models;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Areas.Admin.Controllers
{
    /// <summary>
    /// https://github.com/maxpavlov/jQuery-File-Upload.MVC3
    /// </summary>
    [AuthorizeAdmin]
    public class MediaController : Controller
    {
        #region - Services -

        [Dependency]
        public IAppService AppService { get; set; }

        [Dependency]
        public IAssetService AssetService { get; set; }

        [Dependency]
        public IAssetCategoryService AssetCategoryService { get; set; }

        #endregion


        [HttpGet]
        public ActionResult Index()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var categories = AssetCategoryService.FindAllByHotelId(currentHotelId);

            var assets = AssetService.FindAllByHotelId(currentHotelId);

            return View(new AssetCategoryModel(categories, assets));
        }

        [HttpGet]
        public ActionResult Folder(int id)
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var categories = AssetCategoryService.FindAllByHotelId(currentHotelId);

            var assetCategory = categories.FirstOrDefault(x => x.Id == id);
            if (assetCategory.IsNull())
            {
                throw new ArgumentNullException("assetCategory", "AssetCategory does not exist.");
            }

            var model = new AssetCategoryModel(assetCategory.Id, categories, assetCategory.Items);

            return View(model);
        }

        [HttpGet, ActionName("add-folder")]
        public ActionResult AddFolder()
        {
            var currentHotelId = AppService.GetCurrentHotelId();
            var categories = AssetCategoryService.FindAllByHotelId(currentHotelId);

            return View(new AssetCategoryModel(categories));
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("add-folder")]
        public ActionResult AddFolder(AssetCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("AssetCategory model is not valid."));
            }

            var currentHotelId = AppService.GetCurrentHotelId();

            var assetCategory = AssetCategory.Create(currentHotelId, model.Name);
            AssetCategoryService.Save(assetCategory);

            return Json(new JsonResultSuccess("Created succesfully.",
                new
                {
                    id = assetCategory.Id,
                    name = assetCategory.Name,
                    created = true
                })
            );
        }

        [HttpGet, ActionName("edit-folder")]
        public ActionResult EditFolder(int id)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();

            var assetCategory = AssetCategoryService.FindById(id, currentOrganizationId);
            if (assetCategory.IsNull())
            {
                throw new ArgumentNullException("assetCategory", "AssetCategory is null.");
            }

            var categories = AssetCategoryService.FindAllByHotelId(currentOrganizationId);

            return View(new AssetCategoryModel(assetCategory.Id, categories));
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("edit-folder")]
        public ActionResult EditFolder(AssetCategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResultError("AssetCategory model is not valid."));
            }

            var currentOrganizationId = AppService.GetCurrentOrganizationId();

            var assetCategory = AssetCategoryService.FindById(model.Id, currentOrganizationId);
            if(assetCategory.IsNull())
            {
                throw new ArgumentNullException("assetCategory", "AssetCategory is null.");
            }

            assetCategory.Name = model.Name;

            AssetCategoryService.Save(assetCategory);

            return Json(new JsonResultSuccess("Updated succesfully."));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteFolder(int id)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();

            var assetCategory = AssetCategoryService.FindById(id, currentOrganizationId);
            if (assetCategory.IsNull())
            {
                throw new ArgumentNullException("assetCategory", "AssetCategory is null.");
            }

            AssetCategoryService.Delete(assetCategory);

            return Json(new JsonResultSuccess("Deleted succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS</remarks>
        /// <param name="id"></param>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int categoryId, IEnumerable<int> ids)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var assetCategory = AssetCategoryService.FindById(categoryId, currentOrganizationId);
            if (assetCategory.IsNull())
            {
                throw new ArgumentNullException("assetCategory", "AssetCategory is null.");
            }

            assetCategory.Items.Remove(x => ids.Contains(x.Id));

            AssetCategoryService.Save(assetCategory);
            

            return Json(new JsonResultSuccess("Deleted succesfully."));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS</remarks>
        /// <param name="id"></param>
        [HttpGet]
        public void Download(string id)
        {
            var filename = id;
            var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS</remarks>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UploadFiles(int id)
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(id, Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }

                return Json(statuses, "text/plain");
            }

            return Json(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        /// Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        /// </remarks>
        /// <param name="fileName"></param>
        /// <param name="request"></param>
        /// <param name="statuses"></param>
        [NonAction]
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            //if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            //var file = request.Files[0];
            //var inputStream = file.InputStream;

            //var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

            //using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            //{
            //    var buffer = new byte[1024];

            //    var l = inputStream.Read(buffer, 0, 1024);
            //    while (l > 0)
            //    {
            //        fs.Write(buffer, 0, l);
            //        l = inputStream.Read(buffer, 0, 1024);
            //    }
            //    fs.Flush();
            //    fs.Close();
            //}
            //statuses.Add(new ViewDataUploadFilesResult()
            //{
            //    name = fileName,
            //    size = file.ContentLength,
            //    type = file.ContentType,
            //    url = "/admin/media/download/" + fileName,
            //    delete_url = "/admin/media/delete/" + fileName,
            //    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
            //    delete_type = "GET",
            //});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        /// Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="statuses"></param>
        [NonAction]
        private void UploadWholeFile(int assetCategoryId, HttpRequestBase request, ICollection<ViewDataUploadFilesResult> statuses)
        {
            var currentOrganizationId = AppService.GetCurrentOrganizationId();
            var assetCategory = AssetCategoryService.FindById(assetCategoryId, currentOrganizationId);

            for (var i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var webImage = new WebImage(file.InputStream);

                //var data = new byte[file.ContentLength];
                //file.InputStream.Read(data, 0, file.ContentLength);

                var data = webImage.GetBytes();
                var asset = Asset.Create(assetCategory, file.FileName, data.Length, data, file.ContentType);

                AssetService.Save(asset);

                statuses.Add(new ViewDataUploadFilesResult
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/admin/media/download/" + file.FileName,
                    delete_url = "/admin/media/delete/" + file.FileName,
                    thumbnail_url = @"data:image/png;base64," + Convert.ToBase64String(asset.Data),
                    delete_type = "GET",
                });
            }
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }

}
