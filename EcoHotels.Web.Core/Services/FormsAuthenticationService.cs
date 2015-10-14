using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace EcoHotels.Web.Core.Services
{
    public interface IFormsAuthenticationService
    {
        HttpCookie CreateAuthCookie(string id, string role);

        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public HttpCookie CreateAuthCookie(string id, string role)
        {
            // http://msdn.microsoft.com/en-us/library/Aa302399

            // Create the authentication ticket
            var authTicket = new FormsAuthenticationTicket(1, id, DateTime.Now, DateTime.Now.AddMinutes(60), false, role);

            // Now encrypt the ticket.
            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            // in place to help guard against XSS hack
            authCookie.HttpOnly = true;
            
            return authCookie;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}