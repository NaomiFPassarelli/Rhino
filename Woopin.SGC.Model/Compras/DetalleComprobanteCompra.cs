using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Compras
{
    public class DetalleComprobanteCompra
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Rubro de Compra")]
        [DoNotValidateOnlyId]
        [DisplayName("Rubro de Compra")]
        public virtual RubroCompra RubroCompra { get; set; }

        [Required(ErrorMessage = "Es Necesario un IVA")]
        [DoNotValidateOnlyId]
        [DisplayName("Tipo de IVA")]
        public virtual ComboItem TipoIva { get; set; }

        public virtual string Descripcion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Total")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Total debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Total { get; set; }

        [Required(ErrorMessage = "Es Necesario un IVA")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El IVA debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal IVA { get; set; }

        [DoNotValidate]
        public virtual ComprobanteCompra Comprobante { get; set; }

        public DetalleComprobanteCompra() 
        {
 
        }
    }
}
