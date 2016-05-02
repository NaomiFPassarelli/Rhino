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
    public class Banco : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Es Necesario el nombre del banco")]
        [DisplayName("Nombre del Banco")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "El lugar es requerido")]
        [DoNotValidateOnlyId]
        public virtual Localizacion Lugar { get; set; }
        public virtual bool Activo { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        
        public Banco()
        {
            this.Activo = true;
        }
    }
}
