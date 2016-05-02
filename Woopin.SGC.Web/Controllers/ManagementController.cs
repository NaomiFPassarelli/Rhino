using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;

namespace Woopin.SGC.Web.Controllers
{
    public class ManagementController : BaseController
    {
        private readonly ISystemService SystemService;
        public ManagementController(ISystemService SystemService)
        {
            this.SystemService = SystemService;
        }

        public ActionResult ApplicationLog()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllLogs(PagingRequest paging, int IdUsuario,int IdOrganizacion, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.SystemService.GetAllLogsByDates(IdUsuario,IdOrganizacion, range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<Log> resp = new PagingResponse<Log>();
                resp.Page = paging.page;
                resp.Records = this.SystemService.GetAllLogsByDates(IdUsuario,IdOrganizacion, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public void ToggleDebugging()
        {
            SessionDataManager.ToggleUserDebugging();
        }

    }
}
