using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using System.Web;

namespace Woopin.SGC.Model.Common
{
    public class Organizacion
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Razon Social")]
        [DisplayName("Razón Social")]
        public virtual string RazonSocial { get; set; }

        [Required(ErrorMessage = "Es Necesario un Nombre de Fantasia")]
        [DisplayName("Nombre de Fantasia")]
        public virtual string NombreFantasia { get; set; }

        [DoNotValidateOnlyId]
        public virtual ComboItem Categoria { get; set; }

        [Required(ErrorMessage = "Es Necesario un CUIT")]
        [RegularExpression(@"[0-9]{2}-[0-9]{8}-[0-9]{1}",
        ErrorMessage = "El CUIT no es valido.")]
        public virtual string CUIT { get; set; }

        [Required(ErrorMessage = "Es Necesario el Nro de Ingresos Brutos")]
        [DisplayName("Ingresos Brutos")]
        public virtual string IngresosBrutos { get; set; }

        [Required(ErrorMessage = "Es Necesario un Domicilio")]
        public virtual string Domicilio { get; set; }

        [DisplayName("Codigo Postal")]
        public virtual string CodigoPostal { get; set; }

        [DoNotValidateOnlyId]
        public virtual Localizacion Provincia { get; set; }

        [Required(ErrorMessage = "Es Necesario un Telefono")]
        public virtual string Telefono { get; set; }

        [Required(ErrorMessage = "Es Necesario un Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "El email no es valido.")]
        public virtual string Email { get; set; }

        [DoNotValidateOnlyId]
        public virtual ComboItem Actividad { get; set; }
        public virtual bool Activo { get; set; }

        public virtual Usuario Administrador { get; set; }
        [DisplayName("Imagen")]
        public virtual string ImagePath { get; set; }

    }

}
