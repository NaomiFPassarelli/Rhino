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
    public class TarjetaCredito : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero")]
        [DisplayName("Número")]
        public virtual string Numero { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero de Cta")]
        [DisplayName("Numero Cuenta")]
        [DoNotValidateOnlyId]
        public virtual CuentaBancaria CuentaBancaria { get; set; }
        public virtual EstadoTarjeta Estado { get; set; }
        [DoNotValidateOnlyId]
        public virtual Cuenta CuentaContable { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }

    public enum EstadoTarjeta
    {
        //Activa = 1,
        //Cancelada = -2,
        //Eliminada = -1
        Eliminada = -1, //anulada
        Activa = 1, //creada
        Cancelada = 10
    }
}
