using System.Diagnostics;
using ImageResizer.Configuration;
using ImageResizer.Plugins;

namespace EcoHotels.Web.Core.ImageResize
{
    /// <summary>
    /// http://imageresizing.net/docs/plugins/basics
    /// </summary>
    public class SimplePlugin : IPlugin
    {
        public IPlugin Install(Config c)
        {
            Trace.WriteLine("Installing SimplePlugin");
             
            c.Plugins.add_plugin(this);
            return this;
        }

        public bool Uninstall(Config c)
        {
            c.Plugins.remove_plugin(this);
            return true; //True for successfull uninstallation, false if we couldn't do a clean job of it.
        }
    }
}
