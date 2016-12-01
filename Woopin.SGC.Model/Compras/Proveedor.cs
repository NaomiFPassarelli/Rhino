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
    public class Proveedor : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Razon Social")]
        [DisplayName("Razón Social")]
        public virtual string RazonSocial { get; set; }

        [Required(ErrorMessage = "Es Necesario una Condición de Compra")]
        [DisplayName("Condición de Compra")]
        [DoNotValidateOnlyId]
        public virtual ComboItem CondicionCompra { get; set; }

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

        [DisplayName("Pais")]
        [DoNotValidateOnlyId]
        public virtual ComboItem Pais { get; set; }

        [DoNotValidateOnlyId]
        [DisplayName("Provincia")]
        public virtual Localizacion Localizacion { get; set; }

        public virtual string Telefono { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        ErrorMessage = "El email no es valido.")]
        public virtual string Email { get; set; }

        [DisplayName("Categoria IVA")]
        [Required(ErrorMessage = "Es Necesario una Categoria de IVA")]
        [DoNotValidateOnlyId]
        public virtual CategoriaIVA CategoriaIva { get; set; }

        [Required(ErrorMessage = "Es Necesario un CUIT")]
        [RegularExpression(@"[0-9]{2}-[0-9]{8}-[0-9]{1}",
        ErrorMessage = "El CUIT no es valido.")]
        public virtual string CUIT { get; set; }

        public virtual bool Interno { get; set; }
        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Proveedor()
        {
            this.Activo = true;
        }
        
    }
}
