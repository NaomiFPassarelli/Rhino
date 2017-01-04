using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Common
{
    public class Sucursal : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public virtual string Nombre { get; set; }
        [Required(ErrorMessage = "La direccion es requerida")]
        public virtual string Direccion { get; set; }
        [Required(ErrorMessage = "La localidad es requerida")]
        public virtual string Localidad { get; set; }
        [Required(ErrorMessage = "El  es requerido")]
        [DisplayName("Código Postal")]
        public virtual string CodigoPostal { get; set; }
        [Required(ErrorMessage = "El telefono es requerido")]
        public virtual string Telefono1 { get; set; }
        public virtual string Telefono2 { get; set; }
        public virtual string Telefono3 { get; set; }
        [Required(ErrorMessage = "El email es requerido")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "El lugar es requerido")]
        [DoNotValidateOnlyId]
        public virtual Localizacion Lugar { get; set; }
        public virtual bool Activo { get; set; }
        public virtual bool Predeterminado { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public Sucursal() 
        {
            this.Activo = true;
        }
    }
}
