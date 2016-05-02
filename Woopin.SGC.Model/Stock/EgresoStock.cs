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
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Stock
{
    public class EgresoStock : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Articulo")]
        [DoNotValidateOnlyId]
        public virtual Articulo Articulo { get; set; }

        //la cantidad depende de la unidad de medida del articulo
        [Required(ErrorMessage = "Es Necesario una Cantidad")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,3})?$", ErrorMessage = "La Cantidad debe ser un número mayor a cero, puede contener el caracter punto (.) y con tres decimales")]
        public virtual decimal Cantidad { get; set; }
        //[DoNotValidate]
        //public virtual ComprobanteCompra Comprobante { get; set; }
        public virtual string Observacion { get; set; }
        public virtual Organizacion Organizacion { get; set; }
        //public virtual UsuarioOrganizacion UsuarioOrganizacion { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public EgresoStock() 
        {
 
        }
    }
}
