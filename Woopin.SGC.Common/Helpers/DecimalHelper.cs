using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.Helpers
{
    public static class DecimalHelper
    {
        public static string ToStringArCurrency(this decimal str)
        {
            return string.Format(new System.Globalization.CultureInfo("es-AR"), "{0:C}", str);
        }
    }
}
