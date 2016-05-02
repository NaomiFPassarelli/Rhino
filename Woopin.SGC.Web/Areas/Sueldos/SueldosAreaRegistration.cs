using System.Web.Mvc;

namespace Woopin.SGC.Web.Areas.Sueldos
{
    public class SueldosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sueldos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sueldos_default",
                "Sueldos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
