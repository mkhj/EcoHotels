using System;
using System.IO;
using System.Web;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core.ImageResize
{
    public class DBImageSettings
    {
        public int Id { get; set; }

        public string Filename { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool HasQueryData { get; set; }

        public static DBImageSettings CreateSettings(HttpContext context)
        {
            var dbImageSettings = CreateSettings(context.Request.Url.AbsolutePath);
            dbImageSettings.HasQueryData = !string.IsNullOrEmpty(context.Request.Url.Query);

            return dbImageSettings;
        }

        public static DBImageSettings CreateSettings(string virtualPath)
        {
            var value = virtualPath.Split('/');
            var sizes = value[3].Split('x');

            //TODO: Ensure default values

            return new DBImageSettings
            {
                Id = value[2].ToInt(),
                Filename = Path.GetFileName(virtualPath),
                Width = sizes[0].ToInt(),
                Height = sizes[1].ToInt(),
                HasQueryData = false
            };
        }
    }
}
