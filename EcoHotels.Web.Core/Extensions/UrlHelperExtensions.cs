using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EcoHotels.Web.Core.Extensions
{
    public static class UrlHelperExtensions
    {
        static string assemblyVersion;

        static UrlHelperExtensions()
        {
            // This way of getting the assembly version works in medium trust.
            //assemblyVersion = new AssemblyName(typeof(StaticFileHelper).Assembly.FullName).Version.ToString();
        }
 
        public static string StaticFile(this UrlHelper html, string filename)
        {
            var virtualPath = (
                html.RequestContext.HttpContext.IsDebuggingEnabled
                    ? DebugVirtualPath(filename, html.RequestContext.HttpContext.Server)
                    : ReleaseVirtualPath(filename)
            );
 
            var root = html.RequestContext.HttpContext.Request.ApplicationPath;
            if (root.Length > 1) // e.g. "/myapp" instead of just "/"
            {
                virtualPath = root + virtualPath;
            }
            return virtualPath;
        }
 
        static string ReleaseVirtualPath(string filename)
        {
            // Insert the assembly version into the path (not the query string).
            // This seems to be more reliable when proxies are involved:
            // http://www.stevesouders.com/blog/2008/08/23/revving-filenames-dont-use-querystring/
            return "/static/" + assemblyVersion + "/" + filename;
        }
 
        static string DebugVirtualPath(string filename, HttpServerUtilityBase server)
        {
            // Use query string to break caching. This means the file's path
            // still matches the development file system.
            var absoluteFilename = server.MapPath("~/static/" + filename);
            var version = File.GetLastWriteTime(absoluteFilename).Ticks.ToString();
            var separator = (filename.Contains("?") ? "&" : "?");
            return "/static/" + filename + separator + "nocache=" + version;
        }
    }
}
