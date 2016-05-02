using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Exceptions
{
    public class SecurityException : Exception
    {
        public string ErrorMessage { get; set; }

        public SecurityException(string msg)
        {
            this.ErrorMessage = msg;
        }

        public SecurityException()
        {
            this.ErrorMessage = "";
        }
    }
}
