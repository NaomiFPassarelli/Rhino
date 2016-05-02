using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Controllers
{
    public class UsuariosController : BaseController
    {

        private readonly ISystemService systemService;
        private readonly ICommonConfigService commonConfigService;
        public UsuariosController(ISystemService systemService, ICommonConfigService commonConfigService)
        {
            this.systemService = systemService;
            this.commonConfigService = commonConfigService;
        }


        //
        // GET: /Usuarios/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Usuario Usuario)
        {
            ClearNotValidatedProperties(Usuario);
            string ErrorMessage = "";
            if (ModelState.IsValid)
            {
                if (this.systemService.GetUsuarioByUsername(Usuario.Username) == null)
                {
                    try
                    {
                        this.systemService.AddUsuario(Usuario);
                        WebSecurity.CreateAccount(Usuario.Username, Usuario.Password);
                        return Json(new { Success = true });
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ErrorMessage = ErrorCodeToString(e.StatusCode);
                    }
                }
                else
                {
                    ErrorMessage = "Ya se encuentra una cuenta creada con ese usuario";
                }
            }
            else
            {
                ErrorMessage = "Esta faltando completar algun campo.";
            }
            return Json(new { Success = false, ErrorMessage = ErrorMessage });
        }


        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.systemService.GetAllUsuarios(), Success = true });
            }
            else
            {
                PagingResponse<Usuario> resp = new PagingResponse<Usuario>();
                resp.Page = paging.page;
                resp.Records = this.systemService.GetAllUsuarios();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }



        [HttpPost]
        public JsonResult EliminarUsuario(List<int> Ids)
        {
            try
            {
                this.systemService.CambiarActivo(Ids, false);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }

        [HttpPost]
        public JsonResult HabilitarUsuario(List<int> Ids)
        {
            try
            {
                this.systemService.CambiarActivo(Ids, true);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }

        [HttpPost]
        public JsonResult GetUsuariosCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.systemService.GetAllUsuariosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetUsuario(int IdUsuario)
        {
            return Json(new { Data = this.systemService.GetUsuario(IdUsuario), Success = true });
        }


        #region Usuarios & Organizaciones

        [HttpPost]
        public JsonResult GetAllUsuariosByOrganizacion(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.systemService.GetAllUsuariosByOrganizacion(Security.GetOrganizacion().Id), Success = true });
            }
            else
            {
                PagingResponse<Usuario> resp = new PagingResponse<Usuario>();
                resp.Page = paging.page;
                resp.Records = this.systemService.GetAllUsuariosByOrganizacion(Security.GetOrganizacion().Id);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }


        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
