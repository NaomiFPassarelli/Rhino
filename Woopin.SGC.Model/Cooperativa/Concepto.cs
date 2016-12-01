using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Cooperativa
{
    public class Concepto : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "El valor es requerido")]
        public virtual string Descripcion { get; set; }
        public virtual string AdditionalDescription { get; set; }
        //[RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Porcentaje debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        //public virtual decimal? Porcentaje { get; set; }
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Valor debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal? Valor { get; set; }
        public virtual bool Suma { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        [Required(ErrorMessage = "El tipo es requerido")]
        [DisplayName("Tipo")]
        public virtual TypeConcepto TipoConcepto { get; set; }

        public Concepto()
        {

        }

        
    }
    public enum TypeConcepto
    {
        Anticipo = 0,
        Descuento = 1,
    }
    
}
