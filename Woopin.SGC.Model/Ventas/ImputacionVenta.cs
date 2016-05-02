using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Ventas
{
    public class ImputacionVenta : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [DoNotValidateOnlyId]
        public virtual ComprobanteVenta NotaCredito { get; set; }

        [DoNotValidateOnlyId]
        public virtual ComprobanteVenta ComprobanteADescontar { get; set; }

        public virtual DateTime Fecha { get; set; }

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        public virtual decimal Importe { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }
}
