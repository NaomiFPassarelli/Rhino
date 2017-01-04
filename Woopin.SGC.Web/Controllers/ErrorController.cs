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
            return View("Index");
        }
        //Lo dejo pero es innecesario porque si pasa esto salta al login directo 
        public ViewResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View("Index");
        }

        public ViewResult Forbidden()
        {
            Response.StatusCode = 403;
            return View("Index");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Index");
        }

        public ViewResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View("Index");
        }
    }
}
