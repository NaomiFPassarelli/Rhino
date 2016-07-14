using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Sueldos
{
    public class AdicionalRecibo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Porcentaje debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Total { get; set; }
        public virtual Organizacion Organizacion { get; set; }

        [DoNotValidate]
        public virtual Adicional Adicional { get; set; }
        
        [DoNotValidateOnlyId]
        public virtual Recibo Recibo { get; set; }
        public virtual decimal? Unidades { get; set; }
        public AdicionalRecibo()
        {
        }
        
    }
    
}
