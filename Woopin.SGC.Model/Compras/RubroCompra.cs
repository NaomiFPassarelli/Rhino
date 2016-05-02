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

namespace Woopin.SGC.Model.Compras
{
    public class RubroCompra : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "La Descripcion es requerida")]
        public virtual string Descripcion { get; set; }

        //[Required(ErrorMessage = "La cuenta es requerida")]
        [DoNotValidate]
        public virtual Cuenta Cuenta { get; set; }

        [DisplayName("Percepcion IIBB")]
        public virtual bool PercepcionIIBB { get; set; }
        [DisplayName("Percepcion IVA")]
        public virtual bool PercepcionIVA { get; set; }
        public virtual bool Activo { get; set; }

        [DisplayName("Categoria")]
        public virtual string CodigoPadre { get; set; } // non persistant propertie

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public RubroCompra() 
        {
            this.Activo = true;
        }
    }
}
