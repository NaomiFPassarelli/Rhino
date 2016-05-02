using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using System.Reflection;

namespace Woopin.SGC.Common.App.Logging
{
    [Serializable]
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class LoggableAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            if(Security.IsUserDebugging())
            {
                // Loggea informacion de entrada
                string info = String.Format("Entrando a {0}.{1}.", args.Method.DeclaringType.Name, args.Method.Name);
                Logger.LogInfo(info);

                Logger.LogDebug(DisplayObjectInfo(args));
            }
            
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            base.OnExit(args);
            if (Security.IsUserDebugging())
            {
               Logger.LogDebug(String.Format("Saliendo del metodo {0}.{1}. Retorna {2}", args.Method.DeclaringType.Name, args.Method.Name, args.ReturnValue));
            }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            base.OnException(args);
            Logger.LogError(String.Format("Exception {0} en {1}, mensaje: {2}", args.Exception, args.Method, args.Exception.Message));
            args.FlowBehavior = FlowBehavior.RethrowException;
        }

        static string DisplayObjectInfo(MethodExecutionArgs args)
	    {
		    StringBuilder sb = new StringBuilder();
		    Type type = args.Arguments.GetType();
		    sb.Append("Method " + args.Method.Name );
		    sb.Append("\r\nArguments:");
		    FieldInfo[] fi = type.GetFields();
		    if (fi.Length > 0)
		    {
			    foreach (FieldInfo f in fi)
			    {
				    sb.Append("\r\n " + f + " = " + f.GetValue(args.Arguments));
			    }
		    }
		    else
			    sb.Append("\r\n None");
 
		    return sb.ToString();
	    }

    }
}
