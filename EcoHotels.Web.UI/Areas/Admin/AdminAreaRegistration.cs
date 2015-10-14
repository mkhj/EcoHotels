using System.Web.Mvc;

namespace EcoHotels.Web.UI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { action = "index", controller = "dashboard", id = UrlParameter.Optional }
            );
        }
    }
}
