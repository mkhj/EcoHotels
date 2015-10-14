using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EcoHotels.Web.Core.Marketing
{
    public class ContentExperimentRoute : System.Web.Routing.Route
    {
        public ContentExperimentRoute(string url)
            : base(url, new MvcRouteHandler())
        {
        }

        public ContentExperimentRoute(string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
        }

        public ContentExperimentRoute(string url, object defaults, object constraints)
            : base(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new MvcRouteHandler())
        {
        }

        public ContentExperimentRoute(string url, object defaults, object constraints, string[] namespaces)
            : base(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new MvcRouteHandler())
        {
            if (namespaces != null && namespaces.Length > 0)
            {
                this.DataTokens = new RouteValueDictionary();
                this.DataTokens["Namespaces"] = namespaces;
            }
        }

        #region - Inherited constructors -

        private ContentExperimentRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
            throw new NotImplementedException();
        }

        private ContentExperimentRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            throw new NotImplementedException();
        }

        private ContentExperimentRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
            throw new NotImplementedException();
        }

        private ContentExperimentRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);

            if (routeData != null)
            {
                var data = routeData.Values["action"] as string;
                var tokens = data.Split('-');

                var action = (tokens.Length > 0) ? tokens[0] : string.Empty;
                routeData.Values["action"] = action;

                var viewPostFix = (tokens.Length == 2) ? tokens[1] : string.Empty;

                if (!string.IsNullOrEmpty(viewPostFix) && viewPostFix.IndexOf("ce", 0, 2) == 0)
                {
                    routeData.Values.Add("view-experiment", action + "." + viewPostFix);
                }
                else
                {
                    routeData.Values.Add("view-experiment", action);
                }
            }

            return routeData;

        }
    }
}
