using System;
using System.Web.Mvc;
using Elmah;

namespace EcoHotels.Web.Core.Attributes
{
    /// <summary>
    /// http://stackoverflow.com/questions/766610/how-to-get-elmah-to-work-with-asp-net-mvc-handleerror-attribute/5936867#5936867
    /// </summary>
    public class HandleErrorWithELMAHAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // Log only handled exceptions, because all other will be caught by ELMAH anyway.
            if (filterContext.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
        }
    }
}
