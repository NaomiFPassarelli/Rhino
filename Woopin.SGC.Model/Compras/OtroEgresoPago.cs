using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Model.Compras
{
    public class OtroEgresoPago
    {
        public virtual int Id { get; set; }
        public virtual ValorIngresado Valor { get; set; }
    }
}
