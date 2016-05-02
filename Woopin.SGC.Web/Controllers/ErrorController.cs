using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            Response.StatusCode = 400;
            return View("Error");
        }
        //Lo dejo pero es innecesario porque si pasa esto salta al login directo 
        public ViewResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View("Error");
        }

        public ViewResult Forbidden()
        {
            Response.StatusCode = 403;
            return View("Error");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Error");
        }

        public ViewResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View("Error");
        }
    }
}
