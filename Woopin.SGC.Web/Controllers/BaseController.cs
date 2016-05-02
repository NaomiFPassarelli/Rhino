using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Web.Filters;
using Woopin.SGC.Web.Models;

namespace Woopin.SGC.Web.Controllers
{
    [InitializeSimpleMembership]
    public class BaseController : Controller
    {
        public BaseController()
        {
            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
            info.NumberFormat.CurrencySymbol = "$";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Count() > 0)
            {
                base.OnActionExecuting(filterContext); // re-added in edit
            }
            else
            {
                if (Session == null || Session[SessionDataFactory.SESSION_KEY] == null)
                {
                    filterContext.Result = new RedirectResult("~/Home/Index");
                }
                //else if (SessionDataManager.Get() != null)
                //{
                //    filterContext.Result = new RedirectResult("~/Home/Index");
                //}
                else
                {
                    // code involving this.Session // edited to simplify
                    base.OnActionExecuting(filterContext); // re-added in edit
                }
            }


        }

        

        protected string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        /*
          Funcion base para descargar un archivo almacenado en el servidor
         */

        public void DescargarArchivo(string path)
        {
            string nombreArchivo = Path.GetFileName(path);
            string archivo = Server.MapPath("~/" + path);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + nombreArchivo + "\"");
            Response.WriteFile(archivo);
            Response.ContentType = "";
            Response.End();
        }

        /* Funcion base para subir un archivo y almacenarlo en la carpeta app_data del servidor */

        protected string GuardarArchivo(HttpPostedFileBase archivo, string carpeta)
        {
            return GuardarArchivo(archivo, carpeta, archivo.FileName);
        }
        protected string GuardarArchivo(HttpPostedFileBase archivo, string folderPath, string filename)
        {
            string path = ""; // absolute path on disk
            string relativePath = ""; // relative path on app

            if (archivo != null)
            {
                // Busca el path hasta el app folder
                path = Server.MapPath("~/");

                // Si se indico una carpeta, la crea, 
                // y la agrega al path relativo a la app
                if (folderPath != null)
                {
                    relativePath += folderPath;

                    if (!Directory.Exists(path + relativePath))
                    {
                        Directory.CreateDirectory(path + relativePath);
                    }
                }

                // Controla que no exista un nombre de archivo igual, devuelve con copia (X)
                string validFilename = GetValidFilename(path + relativePath + "/" + filename);
                
                // Agrega a los paths el nombre del archivo
                relativePath +=  "/" + validFilename;
                path += relativePath;

                // Guarda el archivo con la ruta absoluta
                archivo.SaveAs(path);
            }

            // Devuelve el path relativo a la App
            return relativePath;
        }

        /* Funcion base para eliminar un archivo que esta almacenado en el servidor */
        protected void EliminarArchivo(string path)
        {
            if (path != null)
            {
                string mappedPath = Path.Combine(Server.MapPath("~/"), path);
                FileInfo fi = new FileInfo(mappedPath);
                if (fi != null)
                {
                    fi.Delete();
                }
            }
        }

        protected string GetValidFilename(string fullPath)
        {
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while (System.IO.File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return Path.GetFileName(newFullPath);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }


        protected void ClearNotValidatedProperties(object model)
        {
            foreach(var key in ValidationHelper.GetNotValidateKeys(model))
            {
                if (ModelState.ContainsKey(key))
                {
                    ModelState[key].Errors.Clear();
                }
                
            }
        }
    }
}
