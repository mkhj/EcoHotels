using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EcoHotels.Web.Core.Attributes
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HandleNotFoundAttribute : IExceptionFilter
    {
        private const string _defaultView = "Error";
        private Type _exceptionType = typeof(Exception);
        private string _master;
        private string _view;

        public string Master
        {
            get
            {
                return this._master ?? string.Empty;
            }
            set
            {
                this._master = value;
            }
        }
        public string View
        {
            get
            {
                if (string.IsNullOrEmpty(this._view))
                {
                    return "Error";
                }
                return this._view;
            }
            set
            {
                this._view = value;
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            Exception exception = filterContext.Exception;
            if (new HttpException(null, exception).GetHttpCode() != 404)
            {
                return;
            }

            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            filterContext.Result = new ViewResult
            {
                ViewName = this.View,
                MasterName = this.Master,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
