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

namespace Woopin.SGC.Model.Reporting
{
    public class GrupoEgreso
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Es Necesario la Descripcion")]
        [DisplayName("Descripción")]
        public virtual string Descripcion { get; set; }
        [Required(ErrorMessage = "Es Necesario la Raiz")]
        public virtual int Raiz { get; set; }
        [Required(ErrorMessage = "Es Necesario el Level")]
        public virtual int Level { get; set; }
        public virtual GrupoEgreso NodoPadre { get; set; }
        [DoNotValidateOnlyId]
        public virtual RubroCompra Rubro { get; set; }
        public virtual Organizacion Organizacion { get; set; }

        public GrupoEgreso()
        {

        }
    }

}
