using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Common
{
    public class ComboOrganizacion : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public virtual string Nombre { get; set; }
        public virtual bool Activo { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public ComboOrganizacion()
        {
            this.Activo = true;
        }

    }


    public enum ComboOrganizacionType
    {
        CategoriasEmpleados = 100,
        TareasEmpleados = 101,
        Sindicato = 102,
        ObraSocial = 103
    }
}
