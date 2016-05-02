using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Services.Afip;
using Woopin.SGC.Web.Filters;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using System.Configuration;
using WebMatrix.WebData;
using Hangfire;
using Woopin.SGC.Common.Helpers;
using Woopin.SGC.Web.Scheduler;
using System.Web.Security;
using Woopin.SGC.CommonApp.Security;

namespace Woopin.SGC.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class HomeController : BaseController
    {
        private readonly ISystemService SystemService;
        //temp
        private readonly IAfipService afipservice;
        private readonly IVentasService ventasService;
        public HomeController(ISystemService SystemService, IVentasService ventasService, IAfipService afipservice)
        {
            this.SystemService = SystemService;
            this.afipservice = afipservice;
            this.ventasService = ventasService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDashboard()
        {
            return Json(new { Dashboard = this.SystemService.GetDashboard() });
        }


        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            try
            {
                if (WebSecurity.CurrentUserId > 0)
                {
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false });
                }
            }
            catch
            {
                return Json(new { Success = false });
            }

        }

    }
}
