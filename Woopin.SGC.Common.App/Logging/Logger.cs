using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.App.Logging
{
    public static class Logger
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger("MainLogger");

        public static void LogError(string message)
        {
            log.Error(message);
        }

        public static void LogError(string message, Exception e)
        {
            log.Error(message, e);
        }

        public static void LogDebug(string message)
        {
            log.Debug(message);
        }

        public static void LogInfo(string message)
        {
            log.Info(message);
        }

        public static void LogFatal(string message)
        {
            log.Fatal(message);
        }

        public static void LogFatal(string message, Exception e)
        {
            log.Fatal(message,e);
        }

        public static void LogWarning(string message)
        {
            log.Warn(message);
        }
    }
}
