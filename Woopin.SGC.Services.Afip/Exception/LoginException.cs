using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Exceptions
{
    public class LoginException : Exception
    {
        public string ErrorMessage { get; set; }

        public LoginException(string msg)
        {
            this.ErrorMessage = msg;
        }

        public LoginException()
        {
            this.ErrorMessage = "";
        }
    }
}
