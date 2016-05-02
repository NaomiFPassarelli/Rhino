using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System.Configuration;
using Hangfire.Dashboard;
using Ninject;

[assembly: OwinStartup(typeof(Woopin.SGC.Web.Startup))]

namespace Woopin.SGC.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfire(config =>
            {
                config.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["Production"].ConnectionString);
                config.UseServer();
                config.UseAuthorizationFilters(new AuthorizationFilter
                {
                    Users = "Administrador"
                });

            });
            Scheduler.Scheduler.InitializeScheduling();
        }

    }
}
