using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Compras
{
    public class OrdenPagoComprobanteItem
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Comprobante")]
        [DoNotValidateOnlyId]
        public virtual ComprobanteCompra ComprobanteCompra { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        public virtual decimal Importe { get; set; }

        //Total Pagado hasta el momento de cancelacion.
        public virtual decimal Pagado { get; set; }
    }
}
