using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Contabilidad
{
    public class LibroIVA
    {
        public DateTime Fecha { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Comprobante { get; set; }
        public string LetraNumero { get; set; }
        public decimal ImporteExento { get; set; }
        public decimal ImporteGravado { get; set; }
        public decimal ImporteIVA105 { get; set; }
        public decimal ImporteIVA21 { get; set; }
        public decimal ImporteIVA27 { get; set; }
        public decimal PercepcionIIBB{ get; set; }
        public decimal PercepcionIVA { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }

    }
}
