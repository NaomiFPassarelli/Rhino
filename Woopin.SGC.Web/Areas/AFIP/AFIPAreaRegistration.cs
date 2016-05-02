using System.Web.Mvc;

namespace Woopin.SGC.Web.Areas.AFIP
{
    public class AFIPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AFIP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AFIP_default",
                "AFIP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
