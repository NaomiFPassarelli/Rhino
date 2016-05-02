using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Ventas
{
    public class VentasCuentaCorriente
    {
        public decimal DeudaVencida { get; set; }
        public decimal DeudaNoVencida { get; set; }
        public decimal Saldo { get; set; }
    }
}
