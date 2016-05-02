using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Ventas
{
    public class ReporteAcumulado
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Articulo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}
