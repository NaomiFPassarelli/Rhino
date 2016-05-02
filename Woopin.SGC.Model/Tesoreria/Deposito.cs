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
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class Deposito : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "Es necesario indicar la cuenta bancaria")]
        public virtual CuentaBancaria Cuenta { get; set; }

        
        [DisplayName("Fecha de Deposito")]
        public virtual DateTime FechaDeposito { get; set; }

        public virtual string Concepto { get; set; }

        [Required(ErrorMessage = "Es Necesario el número de Boleta")]
        [DisplayName("Número de Boleta")]
        public virtual string NumeroBoleta { get; set; } 

        [DisplayName("Fecha de Acreditación")]
        public virtual DateTime FechaAcreditacion { get; set; }

        public virtual DateTime FechaCreacion { get; set; }

        public virtual IList<DepositoItem> Cheques { get; set; }
        public virtual decimal Total { get; set; }

        public virtual Asiento Asiento { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Deposito()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
