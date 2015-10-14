using System;
using System.IO;
using System.Web.Hosting;
using ImageResizer.Plugins;

namespace EcoHotels.Web.Core.ImageResize
{
    public class DBImage : VirtualFile, IVirtualFileWithModifiedDate, IVirtualFile
    {
        private DBImagePlugin Provider;
        private DateTime? FileModifiedDate = null;

        private DBImageSettings Settings;

        public DBImage(string virtualPath, DBImagePlugin provider) : base(virtualPath)
        {
            Provider = provider;
            Settings = DBImageSettings.CreateSettings(virtualPath);
        }

        public override Stream Open()
        {
            return Provider.GetStream(Settings.Id, Settings.Filename);
        }

        public DateTime ModifiedDateUTC
        {
            get
            {
                if (!FileModifiedDate.HasValue)
                {
                    FileModifiedDate = new DateTime?(Provider.GetDateModifiedUtc(Settings.Id, Settings.Filename));
                }

                return FileModifiedDate.Value;
            }
        }
    }
}
