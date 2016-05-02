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

namespace Woopin.SGC.Model.Ventas
{
    public class Cliente : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Razon Social")]
        [DisplayName("Razón Social")]
        public virtual string RazonSocial { get; set; }

        [Required(ErrorMessage = "Es Necesario una Dirección")]
        [DisplayName("Dirección")]
        public virtual string Direccion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero")]
        [DisplayName("Número")]
        public virtual string Numero { get; set; }
        public virtual string Piso { get; set; }
        public virtual string Departamento { get; set; }

        [DisplayName("Codigo Postal")]
        [Required(ErrorMessage = "Es Necesario un Codigo Postal")]
        [DataType(DataType.PostalCode,ErrorMessage="Codigo postal invalido")]
        public virtual string CodigoPostal { get; set; }

        [Required(ErrorMessage = "Es Necesario una Localidad")]
        public virtual string Localidad { get; set; }

        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "Es Necesario una Localizacion")]
        public virtual Localizacion Localizacion { get; set; }

        public virtual string Telefono { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage = "El email no es valido.")]
        public virtual string Email { get; set; }

        [DisplayName("Categoria IVA")]
        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "Es Necesario una Categoria de IVA")]
        public virtual CategoriaIVA CategoriaIva { get; set; }

        [Required(ErrorMessage = "Es Necesario un CUIT")]
        [RegularExpression(@"[0-9]{2}-[0-9]{8}-[0-9]{1}", ErrorMessage = "El CUIT no es valido.")]
        public virtual string CUIT { get; set; }

        [DoNotValidateOnlyId]
        [Required(ErrorMessage = "Es Necesario una Condición de Venta")]
        [DisplayName("Condición Venta")]
        public virtual ComboItem CondicionVenta { get; set; }

        [DisplayName("Condición Venta Contratada")]
        public virtual int? CondicionVentaContratada { get; set; }

        [DisplayName("Condición Venta Estadistica")]
        public virtual int? CondicionVentaEstadistica { get; set; }

        [DoNotValidate]
        [DisplayName("Empresa Controlante")]
        public virtual GrupoEconomico Master { get; set; }
        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Cliente()
        {
            this.Activo = true;
        }
    }
}
