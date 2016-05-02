using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Compras
{
    public class ReporteComprasRubros
    {
        public int IdRubro { get; set; }
        public string Descripcion { get; set; }
        public decimal Total { get; set; }
        public string MonthGroup { get; set; }
        public int MonthCode { get; set; }
        public int Mes { get; set; }
        public int Year { get; set; }
    }
}
