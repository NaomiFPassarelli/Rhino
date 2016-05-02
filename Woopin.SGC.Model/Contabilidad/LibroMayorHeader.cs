using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Contabilidad
{
    public class LibroMayorHeader
    {
        public virtual decimal Debe { get; set; }
        public virtual decimal Haber { get; set; }
        public virtual decimal Saldo { get; set; }
        public virtual decimal SaldoAnterior { get; set; }
    }
}
