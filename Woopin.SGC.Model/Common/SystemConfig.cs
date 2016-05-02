using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public static class SystemConfig
    {
        public static bool DebugEnabled { get; set; }
        


        static SystemConfig() 
        {
            DebugEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsDebugEnable"]);
        }

    }
}
