using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class Chequera : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Banco")]
        [DoNotValidateOnlyId]
        public virtual CuentaBancaria CuentaBancaria { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero Desde")]
        [DisplayName("Numero Desde")]
        public virtual int NumeroDesde { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero Hasta")]
        [DisplayName("Numero Hasta")]
        public virtual int NumeroHasta { get; set; }

        [Required(ErrorMessage = "Es Necesario un Nombre")]
        public virtual string Nombre { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }

}
