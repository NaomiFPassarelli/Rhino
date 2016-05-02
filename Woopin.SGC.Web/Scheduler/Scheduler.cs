using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woopin.SGC.Services;

namespace Woopin.SGC.Web.Scheduler
{
    public class Scheduler
    {
        private readonly ISystemService systemService;

        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Scheduler(ISystemService systemService)
        {
            this.systemService = systemService;
        }

        public static void InitializeScheduling()
        {
            //RecurringJob.AddOrUpdate<VentasJobs>(x => x.RecordarVencimientos(), Cron.Daily);
        }

       
    }
}