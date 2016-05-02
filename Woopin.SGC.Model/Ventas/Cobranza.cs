using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Model.Ventas
{
    public class Cobranza : ISecuredEntity
    {
        public virtual int Id { get; set; }

        public virtual int NumeroReferencia { get; set; }

        [Required(ErrorMessage = "El Cliente es requerido")]
        [DoNotValidateOnlyId]
        public virtual Cliente Cliente { get; set; }

        [Required(ErrorMessage = "El Número de Recibo es requerido")]
        [DisplayName("Número de Recibo")]
        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "El Estado es requerido")]
        public virtual EstadoComprobanteCancelacion Estado { get; set; }
        
        public virtual string Detalle { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime Fecha { get; set; }
        public virtual DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage="Es necesario especificar el tipo de cobranza")]
        public virtual ComboItem Tipo { get; set; }

        [Required(ErrorMessage="Es necesario el total de la cobranza")]
        public virtual decimal Total { get; set; }

        [Required(ErrorMessage = "Es necesario agregar valores a la cobranza")]
        [DoNotValidateOnlyId]
        public virtual IList<CobranzaValorItem> Valores { get; set; }

        [Required(ErrorMessage = "Es Necesario un Comprobante")]
        [DoNotValidateOnlyId]
        public virtual IList<CobranzaComprobanteItem> Comprobantes { get; set; }

        public virtual Asiento Asiento { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Cobranza() 
        {
        }
    }
}
