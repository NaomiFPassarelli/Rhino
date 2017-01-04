using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Contabilidad
{
    public class Retencion : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "La Abreviatura es requerida")]
        public virtual string Abreviatura { get; set; }

        [Required(ErrorMessage = "Es Necesario una Descripción")]
        [DisplayName("Descripción")]
        public virtual string Descripcion { get; set; }

        [Required(ErrorMessage = "La juridiccion es requerida")]
        [DoNotValidateOnlyId]
        public virtual Localizacion Juridiccion { get; set; }

        // Cuenta contable del Activo a utilizar cuando los clientes retienen.
        [DoNotValidate]
        public virtual Cuenta CuentaContable { get; set; }

        // Cuenta contable del Pasivo a utilizar cuando a los proveedores le retienen.
        [DoNotValidate]
        public virtual Cuenta CuentaADepositar { get; set; }
        public virtual bool Activo { get; set; }

        [DisplayName("Representa un Valor?")]
        public virtual bool EsValor { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }


        public Retencion()
        {
            this.Activo = true;
        }
    }
}
