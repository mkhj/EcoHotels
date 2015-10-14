using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core.Marketing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ContentExperimentAttribute : FilterAttribute, IActionFilter, IResultFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception.IsNotNull())
            {
                return;
            }

            if (!(filterContext.Result is ViewResult))
            {
                return;
                //throw new InvalidOperationException("You need to call the View method");
            }

            var viewName = filterContext.RouteData.Values["view-experiment"] as string;
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ApplicationException("The View Experiment has not been set.");
            }

            var view = (ViewResult)(filterContext.Result);
            view.ViewName = viewName;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //TODO: test if this is works.. one or the other
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }
    }
}
