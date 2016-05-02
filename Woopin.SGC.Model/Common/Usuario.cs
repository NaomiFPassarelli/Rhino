using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Common
{
    public class Usuario
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es necesario un {0}")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Nombre de Usuario")]
        public virtual string Username { get; set; }

        [Required(ErrorMessage = "Es necesario un {0}")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [Display(Name= "Nombre Completo")]
        public virtual string NombreCompleto { get; set; }
        public virtual bool Activo { get; set; }
        public virtual DateTime LastLogin { get; set; }

        [Required(ErrorMessage = "Es necesario una {0}")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public virtual string Password { get; set; }

        [Required(ErrorMessage = "Es necesario un {0}")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public virtual string Email { get; set; }

        [DoNotValidate]
        public virtual Organizacion OrganizacionActual { get; set; }

        public virtual bool IsDebugging { get; set; }

        public Usuario()
        {
            this.Activo = true;
        }
    }
}
