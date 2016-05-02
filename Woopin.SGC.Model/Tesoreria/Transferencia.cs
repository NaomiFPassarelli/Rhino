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
    public class Transferencia : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero de Cta")]
        [DisplayName("Numero Cuenta")]
        [DoNotValidateOnlyId]
        public virtual CuentaBancaria CuentaBancaria { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        [DisplayName("Monto")]
        public virtual decimal Importe { get; set; }

        [DisplayName("Fecha Emisión")]
        public virtual DateTime Fecha { get; set; }
        public virtual DateTime FechaCreacion { get; set; }

        [DoNotValidate]
        public virtual Cliente Cliente { get; set; }
        [DoNotValidate]
        public virtual Proveedor Proveedor { get; set; }

        public virtual EstadoTransferencia Estado { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }


    public enum EstadoTransferencia
    {
        //Anulado = -2,
        //Borrador = -1,
        //Creado = 0,
        Anulado = -1,
        Borrador = 0,
        Creado = 1,
    }

}
