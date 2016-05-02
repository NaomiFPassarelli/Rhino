using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Exceptions
{
    public class BusinessException : Exception
    {
        public string ErrorMessage { get; set; }

        public BusinessException(string msg)
        {
            this.ErrorMessage = msg;
        }

        public BusinessException()
        {
            this.ErrorMessage = "";
        }
    }
}
