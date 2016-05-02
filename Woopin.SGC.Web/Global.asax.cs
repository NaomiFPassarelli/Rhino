using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Web.App_Start;

namespace Woopin.SGC.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            log4net.Config.XmlConfigurator.Configure();
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }

        protected void Session_Start()
        {

        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg == "Organizacion")
            {
                return "Organizacion=" + Security.GetOrganizacion().Id;
            }

            return base.GetVaryByCustomString(context, arg);
        }
    }
}