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
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class ChequePropio : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero")]
        public virtual int Numero { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero de Cta")]
        [DisplayName("Numero Cuenta")]
        [DoNotValidateOnlyId]
        public virtual CuentaBancaria CuentaBancaria { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        [DisplayName("Monto")]
        public virtual decimal Importe { get; set; }

        [DisplayName("Fecha Emisión")]
        public virtual DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Es Necesario una fecha de creación")]
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }
        [DisplayName("Fecha De Pago")]
        public virtual DateTime? FechaPago { get; set; }
        public virtual EstadoCheque Estado { get; set; }

        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "El Proveedor es necesario")]
        public virtual Proveedor Proveedor { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }

}
