using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Stock
{
    public class RubroArticulo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "La Descripcion es requerida")]
        [DisplayName("Descripción")]
        public virtual string Descripcion { get; set; }

        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public RubroArticulo() 
        {
            this.Activo = true;
        }
    }
}
