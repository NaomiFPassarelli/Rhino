using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Bolos
{
    public class DetalleLiquidador : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Concepto")]
        [DoNotValidateOnlyId]
        [DisplayName("Concepto Bolo")]
        public virtual ConceptoBolo ConceptoBolo { get; set; }

        //[Required(ErrorMessage = "Es Necesario un IVA")]
        //[DoNotValidateOnlyId]
        //[DisplayName("Tipo de IVA")]
        //public virtual ComboItem TipoIva { get; set; }
        //[DisplayName("Descripción")]
        //public virtual string Descripcion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Total")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Total debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Total { get; set; }

        //[Required(ErrorMessage = "Es Necesario un IVA")]
        //[RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El IVA debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        //public virtual decimal IVA { get; set; }

        [DoNotValidate]
        public virtual Liquidador Liquidador { get; set; }


        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public DetalleLiquidador() 
        {
 
        }
    }
}
