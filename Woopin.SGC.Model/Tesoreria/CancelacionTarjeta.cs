using DataAnnotationsExtensions;
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

namespace Woopin.SGC.Model.Tesoreria
{
    public class CancelacionTarjeta
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Tarjeta")]
        [DoNotValidateOnlyId]
        public virtual PagoTarjeta Pago { get; set; }

        [Required(ErrorMessage = "Es Necesaria la cantidad de Cuotas")]
        [Range(0, 100, ErrorMessage = "Debe cancelar menos o igual cuotas de las que quedan pendientes")]
        public virtual int Cuotas { get; set; }


        [Required(ErrorMessage = "Es Necesario un Importe")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Debe cancelar menos o igual importe del total pendiente")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Total debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Importe { get; set; }
        public virtual ValorIngresado Valor { get; set; }
        public virtual DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Es Necesario una fecha de creación")]
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }
        public virtual Asiento Asiento { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
    }
}
