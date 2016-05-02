using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Woopin.SGC.Model.Common
{
    public class Moneda
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage="Es necesario un Nombre")]
        [MaxLength(500,ErrorMessage="Maximo 500 caracteres.")]
        public virtual string Nombre { get; set; }

        [MaxLength(500, ErrorMessage = "Maximo 500 caracteres.")]
        [Required(ErrorMessage = "Es necesario una Abreviatura")]
        public virtual string Abreviatura { get; set; }

        [MaxLength(500, ErrorMessage = "Maximo 500 caracteres.")]
        [Required(ErrorMessage = "Es necesario un Codigo de Afip")]
        public virtual string CodigoAfip { get; set; }

        [MaxLength(10, ErrorMessage = "Maximo 10 caracteres.")]
        [Required(ErrorMessage = "Es necesario un Codigo de Afip")]
        public virtual string Signo { get; set; }
        public virtual bool Predeterminado { get; set; }
        public virtual bool Activo { get; set; }
        public Moneda()
        {
            this.Activo = true;
            this.Predeterminado = false;
        }
    }
}
