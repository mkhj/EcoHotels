using System;
using System.Web;
using EcoHotels.Core.Infrastructure.NH;

namespace EcoHotels.Web.Core
{
    /// <summary>
    /// You will need to configure this module in the web.config file of your
    /// web and register it with IIS before being able to use it. For more information
    /// see the following link: http://go.microsoft.com/?linkid=8101007
    /// </summary>
    public class SessionScopeWebModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        #endregion

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            SessionFactory.BeginTransaction();
        }

        private static void OnEndRequest(object sender, EventArgs e)
        {
            SessionFactory.EndTransaction();
        }
    }
}
