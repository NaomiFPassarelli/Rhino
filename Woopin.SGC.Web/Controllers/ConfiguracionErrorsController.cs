using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Woopin.SGC.Web.Controllers
{
    public class ConfiguracionErrorsController : BaseController
    {
        public ActionResult SinCajas()
        {
            return View();
        }

        public ActionResult SinCuentasBancarias()
        {
            return View();
        }
        public ActionResult SinBancos()
        {
            return View();
        }

        public ActionResult SinRetenciones()
        {
            return View();
        }

        public ActionResult SinTarjetas()
        {
            return View();
        }
    }
}
