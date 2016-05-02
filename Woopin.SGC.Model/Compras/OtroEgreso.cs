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
    public class OtroEgreso : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [DisplayName("N° Referencia")]
        public virtual int NumeroReferencia { get; set; }

        [Required(ErrorMessage = "Es Necesario el Proveedor")]
        [DoNotValidateOnlyId]
        public virtual Proveedor Proveedor { get; set; }
        public virtual string Observacion { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha")]
        public virtual DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Contable")]
        public virtual DateTime FechaContable { get; set; }
        public virtual DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Estado")]
        [DisplayName("Estado del Comprobante")]
        public virtual EstadoComprobante Estado { get; set; }
        public virtual decimal Total { get; set; }
        public virtual IList<OtroEgresoItem> Detalle { get; set; }
        public virtual IList<OtroEgresoPago> Pagos { get; set; }

        [DoNotValidate]
        public virtual Asiento Asiento { get; set; }

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }
}
