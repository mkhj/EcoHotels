using System;
using System.Web;
using EcoHotels.Extensions;

namespace EcoHotels.Web.Core.Services
{
    public interface IAppService
    {
        void SetCurrentOrganizationId(int id);

        int GetCurrentOrganizationId();

        int GetCurrentHotelId();

        void SetCurrentHotelId(int id);

        void AbondonSession();        
    }

    public class AppService : IAppService
    {
        private const string ORGANIZATION_STATE_KEY = "ORGANIZATION_STATE_KEY";
        private const string HOTEL_STATE_KEY = "HOTEL_STATE_KEY";

        public AppService()
        {
        }

        #region - Session Handling -

        public void SetCurrentHotelId(int id)
        {
            HttpContext.Current.Session.Add(HOTEL_STATE_KEY, id);
        }

        public int GetCurrentHotelId()
        {
            var value = HttpContext.Current.Session[HOTEL_STATE_KEY];
            if (value.IsNull())
            {
                return 0;
            }

            return (int)value;
        }


        public void SetCurrentOrganizationId(int id)
        {
            HttpContext.Current.Session.Add(ORGANIZATION_STATE_KEY, id);
        }

        public int GetCurrentOrganizationId()
        {
            var value = HttpContext.Current.Session[ORGANIZATION_STATE_KEY];
            if (value.IsNull())
            {
                return 0;
            }

            return (int) value;
        }

        public void AbondonSession()
        {
            HttpContext.Current.Session.Abandon();
        }

        #endregion

        

    }
}