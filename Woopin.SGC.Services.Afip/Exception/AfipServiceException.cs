using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Exceptions
{
    public class AfipServiceException : Exception
    {
        public string ErrorMessage { get; set; }

        public AfipServiceException(string msg)
        {
            this.ErrorMessage = msg;
        }

        public AfipServiceException()
        {
            this.ErrorMessage = "";
        }
    }
}
