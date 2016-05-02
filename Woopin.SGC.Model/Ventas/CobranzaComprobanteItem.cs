using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Ventas
{
    public class CobranzaComprobanteItem
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Comprobante")]
        [DoNotValidateOnlyId]
        public virtual ComprobanteVenta ComprobanteVenta { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        public virtual decimal Importe { get; set; }

        //Total Cobrado hasta el momento de cancelacion.
        public virtual decimal Cobrado { get; set; }
    }
}
