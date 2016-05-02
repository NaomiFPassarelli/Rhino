using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Exceptions
{
    public class ValidationException : Exception
    {
        public string ErrorMessage { get; set; }

        public ValidationException(string msg)
        {
            this.ErrorMessage = msg;
        }

        public ValidationException()
        {
            this.ErrorMessage = "";
        }
    }
}
