using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Stock
{
    public class Articulo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "La Descripcion es requerida")]
        public virtual string Descripcion { get; set; }

        [DisplayName("Código de Barras")]
        public virtual string CodigoBarras { get; set; }

        [DoNotValidateOnlyId]
        public virtual RubroArticulo Rubro { get; set; }

        [DoNotValidateOnlyId]
        [DisplayName("Unidad de Medida")]
        public virtual ComboItem UnidadMedida { get; set; }

        [DoNotValidateOnlyId]
        [DisplayName("Alicuota de IVA")]
        public virtual ComboItem AlicuotaIVA { get; set; }

        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        [Required(ErrorMessage="Es necesario que indique el tipo de articulo")]
        [DisplayName("Tipo de Articulo")]
        public virtual TipoArticulo Tipo { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el estado cuando lleva inventario")]
        public virtual EstadoArticulo Estado { get; set; }
        public virtual bool Inventario { get; set; }
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,3})?$", ErrorMessage = "El Stock debe ser un número mayor a cero, puede contener el caracter punto (.) y con tres decimales")]
        public virtual decimal? Stock { get; set; }
        public Articulo() 
        {
            this.Activo = true;
        }
    }

    public enum TipoArticulo
    {
        Producto = 0,
        Servicio = 1
    }

    public enum EstadoArticulo
    {
        MateriaPrima = 0,
        EnProceso = 1,
        Terminado = 2
    }
}
