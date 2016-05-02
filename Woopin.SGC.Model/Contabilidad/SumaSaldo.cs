using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Contabilidad
{
    public class SumaSaldo
    {
        public string Codigo { get; set; }
        public string NombreCuenta { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal Saldo { get; set; }
        public decimal SaldoActual { get; set; }
        public int CuentaId { get; set; }
        public int Rubro { get; set; }
        public int Corriente { get; set; }
        public int SubRubro { get; set; }
        public int Numero { get; set; }
    }
}
