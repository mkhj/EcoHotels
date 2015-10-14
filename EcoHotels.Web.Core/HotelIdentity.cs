using System;
using System.Security.Principal;
using System.Web.Security;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core
{
    [Serializable]
    public class HotelIdentity : IIdentity
    {
        private FormsAuthenticationTicket ticket;

        public HotelIdentity(FormsAuthenticationTicket ticket)
        {
            this.ticket = ticket;
        }

        public string Name
        {
            get { return ticket.Name; }
        }

        public string AuthenticationType
        {
            get { return "User"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }
    }
}