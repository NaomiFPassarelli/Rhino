using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class CuentaCorrienteItem
    {
        public string TipoComprobante { get; set; }
        public int NroReferencia { get; set; }
        public string LetraNumero { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal Pendiente { get; set; }
        public string Entidad { get; set; }
        public string Empresa { get; set; }

        public CuentaCorrienteItem()
        {

        }

    }
}
