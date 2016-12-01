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

namespace Woopin.SGC.Model.Common
{
    public class Direccion
    {
        public virtual int Id { get; set; }

        [DisplayName("Calle")]
        public virtual string Direccion { get; set; }

        [DisplayName("Número")]
        public virtual string Numero { get; set; }
        public virtual string Piso { get; set; }
        public virtual string Departamento { get; set; }

        [DisplayName("Codigo Postal")]
        [DataType(DataType.PostalCode, ErrorMessage = "Codigo postal invalido")]
        public virtual string CodigoPostal { get; set; }

        [DoNotValidateOnlyId]
        public virtual Localidad Localidad { get; set; }

        [DoNotValidateOnlyId]
        [DisplayName("Provincia")]
        public virtual Localizacion Localizacion { get; set; }

        [DisplayName("Pais")]
        [DoNotValidateOnlyId]
        public virtual ComboItem Pais { get; set; }

        public virtual string Telefono { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "El email no es valido.")]
        public virtual string Email { get; set; }

        public Direccion()
        {

        }

    }
}
