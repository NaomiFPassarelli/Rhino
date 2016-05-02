using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Models;

namespace Woopin.SGC.Web.Controllers
{
    public class MiCuentaController : BaseController
    {
        private readonly ISystemService systemService;
        public MiCuentaController(ISystemService systemService)
        {
            this.systemService = systemService;
        }

        //
        // GET: /MiCuenta/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(LocalPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("Las contraseñas no coinciden o no son validas");
                }
                var token = WebSecurity.GeneratePasswordResetToken(WebSecurity.CurrentUserName);
                WebSecurity.ResetPassword(token, model.ConfirmPassword);
                return Json(new { Success = true, SuccessMessage = "La contraseña fue cambiada con exito" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, ErrorMessage = e.Message });

            }
        }

    }
}
