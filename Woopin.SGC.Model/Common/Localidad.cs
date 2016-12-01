using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Common
{
    public class Localidad
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage="Es Necesario un Nombre")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "Es Necesario una Provincia")]
        public virtual Localizacion Provincia { get; set; }
        public virtual bool Activo { get; set; }
        public virtual bool Predeterminado { get; set; }
        public Localidad()
        {
            this.Activo = true;
            this.Predeterminado = false;
        }
    }
}
