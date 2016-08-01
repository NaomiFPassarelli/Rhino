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
    public class Pago : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [DoNotValidateOnlyId]
        public virtual Asociado Asociado { get; set; }
        [Required(ErrorMessage = "Es Necesario una fecha de creacion")]
        public virtual DateTime FechaCreacion { get; set; }
        [Required(ErrorMessage = "Es Necesario una fecha de pago")]
        public virtual DateTime FechaPago { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        [Required]
        public virtual int NumeroCuota { get; set; }
        public Pago()
        {
            this.FechaCreacion = DateTime.Now;
        }
    }
}
