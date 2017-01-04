using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Model.Compras
{
    public class OrdenPago : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "El Proveedor es requerido")]
        [DoNotValidateOnlyId]
        public virtual Proveedor Proveedor { get; set; }

        [Required(ErrorMessage = "El Número es requerido")]
        [DisplayName("N° de Referencia")]
        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime Fecha { get; set; }

        [Required(ErrorMessage="El total no fue calculado")]
        public virtual decimal Total { get; set; }

        public virtual string Detalle { get; set; }

        [Required(ErrorMessage = "Es Necesario un Tipo de Orden de Pago")]
        [DoNotValidateOnlyId]
        public virtual ComboItem Tipo { get; set; }
        public virtual EstadoComprobanteCancelacion Estado { get; set; }
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }

        public virtual IList<OrdenPagoComprobanteItem> Comprobantes { get; set; }
        public virtual IList<OrdenPagoValorItem> Pagos { get; set; }

        public virtual Asiento Asiento { get; set; }

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public OrdenPago()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
