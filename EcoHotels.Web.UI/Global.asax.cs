using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Core.Infrastructure.Services.Impl;
using EcoHotels.Extensions;
using EcoHotels.Web.Core;
using EcoHotels.Web.Core.Attributes;
using EcoHotels.Web.Core.ImageResize;
using EcoHotels.Web.Core.Marketing;
using ImageResizer.Configuration;

namespace EcoHotels.Web.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            this.BeginRequest += MvcApplication_BeginRequest;
            this.EndRequest += MvcApplication_EndRequest;

            this.AuthenticateRequest += MvcApplication_AuthenticateRequest;
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        private void MvcApplication_BeginRequest(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Request.IsLocal && HttpContext.Current.IsDebuggingEnabled)
            {
                var stopwatch = new Stopwatch();
                HttpContext.Current.Items["Stopwatch"] = stopwatch;
                stopwatch.Start();

                var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
                if (routeData.IsNotNull() && routeData.Values.ContainsKey("lang"))
                {
                    var lang = routeData.GetRequiredString("lang");
                    var language = new LanguageService(null).FindByShortName(lang);

                    var ci = new CultureInfo(language.Shortname);
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(ci.Name);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);   
                }
                
            }
        }

        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Response.ContentType == "text/html")
            {
                //if (HttpContext.Current.Request.IsLocal && HttpContext.Current.IsDebuggingEnabled)
                {
                    var stopwatch = (Stopwatch)HttpContext.Current.Items["Stopwatch"];
                    stopwatch.Stop();

                    var ts = stopwatch.Elapsed;
                    var elapsedTime = String.Format("Generating page took {0:f2} seconds", ts.TotalMilliseconds / 1000);

                    HttpContext.Current.Response.Write("\n  <!--" + elapsedTime + "-->");
                }
            }
        }



        protected void Application_Start()
        {
            DependencyResolver.SetResolver(new UnityDependencyResolver());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterViewEngines(ViewEngines.Engines);            
        }


        private void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        private void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                string encTicket = authCookie.Value;
                if (!string.IsNullOrEmpty(encTicket))
                {
                    FormsAuthenticationTicket authTicket = null;
                    try
                    {
                        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    }
                    catch (Exception ex)
                    {
                        // Log exception details (omitted for simplicity)
                        return;
                    }

                    if (null == authTicket)
                    {
                        // Cookie failed to decrypt.
                        return;
                    }

                    // When the ticket was created, the UserData property was assigned a
                    // pipe delimited string of role names.
                    var roles = authTicket.UserData.Split(new[] { '|' });

                    // Create an Identity object
                    var id = new HotelIdentity(authTicket);

                    // This principal will flow throughout the request.
                    var principal = new GenericPrincipal(id, roles);

                    // Attach the new principal object to the current HttpContext object
                    Context.User = principal;
                }
            }
        }


        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorWithELMAHAttribute());
            //filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("elmah.axd");

            routes.IgnoreRoute("{*allcss}", new { allaspx = @".*\.css(/.*)?" });
            routes.IgnoreRoute("{*alljs}", new { allaspx = @".*\.js(/.*)?" });



            routes.MapRoute(
                "SearchRoute", // Route name
                "{lang}/search/{city}/{arrival}/{departure}", // URL with parameters
                new { controller = "search", action = "index", city = 0, arrival= DateTime.MinValue, departure=DateTime.MinValue }, // Parameter defaults
                new { lang = ".{2}"},
                new[] { "EcoHotels.Web.UI.Controllers" }
            );


            #region - HOTEL ROUTES -

            routes.MapRoute(
                "HotelSearchRoute", // Route name
                "{lang}/hotel/search", // URL with parameters
                new { controller = "hotel", action = "search" }, // Parameter defaults
                new { lang = ".{2}" },
                new[] { "EcoHotels.Web.UI.Controllers" }
                );

            routes.MapRoute(
                "HotelRoute", // Route name
                "{lang}/hotel/{id}/{arrival}/{departure}", // URL with parameters
                new { controller = "hotel", action = "index", id = string.Empty, arrival = UrlParameter.Optional, departure = UrlParameter.Optional }, // Parameter defaults
                new { lang = ".{2}" },
                new[] { "EcoHotels.Web.UI.Controllers" }
                );

            #endregion

            routes.Add("Localized_Url", new ContentExperimentRoute(
                "{lang}/{controller}/{action}/{id}", // URL with parameters
                new { controller = "home", action = "index", id = UrlParameter.Optional }, // Parameter defaults
                new { lang = ".{2}" },
                new[] { "EcoHotels.Web.UI.Controllers" }
            ));

            routes.MapRoute(
                "Sitemap", // Route name
                "sitemap", // URL with parameters
                new { controller = "sitemap", action = "index" } // Parameter defaults
            );

            //NOTE: If nothing matches, then we most be hitting the root of the site, so we need to redirect to the correct language
            routes.MapRoute(
                "Default", // Route name
                "{action}", // URL with parameters
                new { controller = "location", action = "index" } // Parameter defaults
            );

            // Fetches images from the database
            Config.Current.Pipeline.Rewrite += delegate(IHttpModule sender, HttpContext context, IUrlEventArgs ev)
            {
                Trace.WriteLine("Resize " + ev.VirtualPath);

                if (ev.VirtualPath.StartsWith(VirtualPathUtility.ToAbsolute("~/img/"), StringComparison.OrdinalIgnoreCase))
                {
                    var settings = DBImageSettings.CreateSettings(context);
                    ev.QueryString["width"] = settings.Width.ToString();
                    ev.QueryString["height"] = settings.Height.ToString();
                }
            };
        }

        public static void RegisterViewEngines(ViewEngineCollection engines)
        {
            engines.Clear();
            engines.Add(new RazorViewEngine());
        }
    }
}