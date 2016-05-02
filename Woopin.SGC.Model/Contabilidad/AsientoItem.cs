using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Model.Contabilidad
{
    public class AsientoItem
    {
        public virtual int Id { get; set; }
        
        [Required(ErrorMessage = "El asiento es requeridos")]
        [DoNotValidateOnlyId]
        public virtual Asiento Asiento { get; set; }
        
        [Required(ErrorMessage = "La cuenta es requeridos")]
        [DoNotValidateOnlyId]
        public virtual Cuenta Cuenta { get; set; }
        public virtual decimal Debe { get; set; }
        public virtual decimal Haber { get; set; }
        public virtual ChequePropio ChequePropio { get; set; } 
        public AsientoItem()
        {

        }
    }
}
