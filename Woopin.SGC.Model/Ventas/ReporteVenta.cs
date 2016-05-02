using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Ventas
{
    public class ReporteVenta
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public string SemanaFecha { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string SemanaVencimiento { get; set; }
        public string Comprobante { get; set; }
        public string TipoComprobante { get; set; }
        public decimal Importe { get; set; }
        public DateTime FechaEstipuladaCobro { get; set; }
        public string SemanaEstipuladaCobro { get; set; }
        public string ContactoCobro { get; set; }
        public string Observacion { get; set; }
    }
}
