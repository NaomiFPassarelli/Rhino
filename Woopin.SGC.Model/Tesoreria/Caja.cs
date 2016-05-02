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
    public class Caja : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario el nombre de la Caja")]
        [DisplayName("Nombre de la Caja")]
        public virtual string Nombre { get; set; }

        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "La cuenta contable es necesaria")]
        public virtual Cuenta CuentaContable { get; set; }

        public virtual decimal Fondos { get; set; }
        public virtual bool Activo { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Caja()
        {
            this.Activo = true;
        }
    }
}
