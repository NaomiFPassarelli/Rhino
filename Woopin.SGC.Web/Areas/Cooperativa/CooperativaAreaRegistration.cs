using System.Web.Mvc;

namespace Woopin.SGC.Web.Areas.Cooperativa
{
    public class CooperativaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cooperativa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Cooperativa_default",
                "Cooperativa/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
