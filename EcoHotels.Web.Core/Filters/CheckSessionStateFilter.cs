using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Hotelr.Web.Core.Filters
{
    public class CheckSessionStateFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isLoginPage = filterContext.RequestContext.RouteData.Values.ContainsValue("account") && filterContext.RequestContext.RouteData.Values.ContainsValue("logon");
            if (isLoginPage)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if(!filterContext.RequestContext.RouteData.Values.ContainsValue("admin"))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // If session exists

            if (filterContext.HttpContext.Session != null)
            {

                //if new session

                if (filterContext.HttpContext.Session.IsNewSession)
                { 
                       
                    var cookie = filterContext.HttpContext.Request.Headers["Cookie"];

                    //if cookie exists and sessionid index is greater than zero

                    if ((cookie != null) &&(cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {

                        FormsAuthentication.SignOut();

                        ////redirect to desired session 

                        //expiration action and controller
                        var redirectTargetDictionary = new RouteValueDictionary();
                        redirectTargetDictionary.Add("action", "logon");
                        redirectTargetDictionary.Add("controller", "account");

                        filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);

                        return;

                    }

                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}