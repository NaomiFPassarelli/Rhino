using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Compras
{
    public class ReporteCompra
    {
        public int Id { get; set; }
        public string Proveedor { get; set; }
        public DateTime Fecha { get; set; }
        public string SemanaFecha { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string SemanaVencimiento { get; set; }
        public string Comprobante { get; set; }
        public string TipoComprobante { get; set; }
        public decimal Importe { get; set; }
    }
}
