using System.Web.Mvc;

namespace Woopin.SGC.Web.Areas.Bolos
{
    public class BolosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Bolos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Bolos_default",
                "Bolos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
