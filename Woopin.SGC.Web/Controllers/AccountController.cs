using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Woopin.SGC.Web.Filters;
using Woopin.SGC.Web.Models;
using Woopin.SGC.Services;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Common.HtmlModel;
using Hangfire;
using Woopin.SGC.Web.Scheduler;
using Woopin.SGC.Services.Common;
using Woopin.SGC.Common.Models;
using System.IO;

namespace Woopin.SGC.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private readonly ISystemService systemService;
        private readonly ICommonConfigService commonConfigService;
        public AccountController(ISystemService systemService, ICommonConfigService commonConfigService)
        {
            this.systemService = systemService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Account/Index

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult EnviarMensaje(string emailContacto, string telefonoContacto, string nombreContacto, string mensajeContacto)
        {
            if (emailContacto != null || telefonoContacto != null )
            {
                EmailerService service = new EmailerService();
                string templatePath = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplates\\ContactoRhino.html";

                WMail mail = new WMail();
                mail.To.Add("naomi.passarelli@woopin.com.ar");
                mail.From = emailContacto;
                mail.Subject = "Contacto desde rhino woopin de " + nombreContacto + telefonoContacto;

                mail.IsHtml = true;
                StreamReader streamReader = new StreamReader(templatePath);
                mail.Message = streamReader.ReadToEnd().Replace("@@NombreContacto@@", nombreContacto)
                                                           .Replace("@@TelefonoContacto@@", telefonoContacto)
                                                           .Replace("@@EmailContacto@@", emailContacto)
                                                           .Replace("@@MensajeContacto@@", mensajeContacto);
                streamReader.Close();
                service.SendEmail(mail);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }


        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken] TODO
        public JsonResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                Usuario user = this.systemService.GetUsuarioByUsername(model.UserName);
                if (user != null && user.Activo)
                {
                    if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                    {
                        user.LastLogin = DateTime.Now;
                        this.systemService.UpdateUsuario(user);
                        this.systemService.InitializeSessionData(user);// - TODO mandarle el usuario que ya esta el objeto aca.
                        //return RedirectToAction("Index", "Home");

                        return Json(new { Success = true, ReturnUrl = Url.Action("Index", "Home") });
                    }
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "El usuario se encuentra bloqueado." });
                }
                return Json(new { Success = false, ErrorMessage = "El usuario y/o la contraseña no coinciden." });
            }
            catch(Exception e)
            {
                return Json(new { Success = false, ErrorMessage = "Error desconocido: " + e.Message });
            }
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        public JsonResult LogOff()
        {
            WebSecurity.Logout();
            return Json(new { Success = true });
        }


        //[AllowAnonymous]
        //public ActionResult MockLogin()
        //{
        //    Usuario user = this.systemService.GetUsuarioByUsername("Administrador");
        //    if (ModelState.IsValid && WebSecurity.Login("Administrador", "Administrador", persistCookie: false))
        //    {
        //        user.LastLogin = DateTime.Now;
        //        this.systemService.UpdateUsuario(user);
        //        this.systemService.InitializeSessionData(user);// - TODO mandarle el usuario que ya esta el objeto aca.
        //    }
        //    return RedirectToAction("Index","home");
        //}




        
    }
}
