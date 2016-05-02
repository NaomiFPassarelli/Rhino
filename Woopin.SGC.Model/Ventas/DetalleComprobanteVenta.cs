using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Model.Ventas
{
    public class DetalleComprobanteVenta
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Cantidad")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El precio unitario debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Cantidad { get; set; }

        [Required(ErrorMessage = "Es Necesario un IVA")]
        [DoNotValidateOnlyId]
        [DisplayName("Tipo de IVA")]
        public virtual ComboItem TipoIva { get; set; }


        [Required(ErrorMessage = "Es Necesario un Servicio")]
        [DoNotValidateOnlyId]
        public virtual Articulo Articulo { get; set; }

        [Required(ErrorMessage = "Es Necesario una Descripcion")]
        public virtual string Descripcion { get; set; }

        [DisplayName("Precio Unitario")]
        [Min(1, ErrorMessage = "El costo debe ser mayor a 0")]
        [Required(ErrorMessage = "Es Necesario un Precio Unitario")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El precio unitario debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal PrecioUnitario { get; set; }
        
        [DisplayName("Neto Gravado")]
        [Required(ErrorMessage = "Es Necesario un Total")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El total debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Total { get; set; }

        public virtual decimal TotalConIVA { get; set; }

        [DisplayName("% Desc.")]
        public virtual int? Descuento { get; set; }

        [DoNotValidate]
        public virtual ComprobanteVenta Comprobante { get; set; }

        public DetalleComprobanteVenta() 
        {
 
        }
    }
}
