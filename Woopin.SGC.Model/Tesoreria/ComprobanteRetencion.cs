using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class ComprobanteRetencion : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un N° Comprobante")]
        [DisplayName("N° Comprobante")]
        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        [DisplayName("Monto")]
        public virtual decimal Total { get; set; }

        [DisplayName("Fecha Emisión")]
        public virtual DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Es Necesario una fecha de creación")]
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }

        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "Es necesario elegir una retención")]
        public virtual Retencion Retencion { get; set; }

        [DoNotValidate]
        public virtual Cliente Cliente { get; set; }

        [DoNotValidate]
        public virtual Proveedor Proveedor { get; set; }
        public virtual EstadoRetencion Estado { get; set; }
        [DisplayName("Número de Retención")]
        public virtual string NumeroRetencion { get; set; } //esto es para el Reporte de ComprobantesRetenciones

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public ComprobanteRetencion()
        {

        }
    }

    public enum EstadoRetencion
    {
        //Anulada = -2,
        //Borrador = -1,
        //Recibido = 1
        Anulada = -1,
        Borrador = 0,
        Recibido = 7
    }
}
