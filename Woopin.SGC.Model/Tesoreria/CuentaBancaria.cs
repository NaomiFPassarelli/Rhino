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
    public class CuentaBancaria : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Es Necesario un número de cuenta")]
        [DisplayName("Número De Cuenta")]
        public virtual string Numero { get; set; }
        [Required(ErrorMessage = "Es Necesario el nombre de la cuenta")]
        [DisplayName("Nombre de la Cuenta")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "Es Necesario el nombre del banco")]
        [DisplayName("Nombre del Banco")]
        [DoNotValidateOnlyId]
        public virtual Banco Banco { get; set; }

        [Required(ErrorMessage = "Es Necesario la moneda")]
        [DisplayName("Moneda de la Cuenta")]
        [DoNotValidateOnlyId]
        public virtual Moneda Moneda { get; set; }

        [DisplayName("Fondo De La Cuenta")]
        public virtual decimal Fondo { get; set; }

        [DisplayName("Emite Cheques")]
        public virtual bool EmiteCheques { get; set; }

        [DoNotValidateOnlyId]
        public virtual Cuenta CuentaContable { get; set; }
        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public CuentaBancaria()
        {
            this.Activo = true;
        }
    }

    public class listaIdDesc
    {
            public int Id{get;set;}
            public string Descripcion {get;set;}
    }
}
