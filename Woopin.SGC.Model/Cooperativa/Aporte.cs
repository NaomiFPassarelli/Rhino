using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Model.Cooperativa
{
    public class Aporte : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [DoNotValidateOnlyId]
        public virtual Asociado Asociado { get; set; }
        [Required(ErrorMessage = "Es Necesario una fecha de creación")]
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        [Required]
        public virtual int NumeroAbono { get; set; }
        public virtual decimal Total { get; set; }
        //[DisplayName("Fecha de Aporte")]
        //[Required(ErrorMessage = "Es Necesario una Fecha de Aporte")]
        //public virtual DateTime FechaAporte { get; set; } //en la que se va a pagar este recibo

        [DisplayName("Fecha de Período de Aporte")]
        [Required(ErrorMessage = "Es Necesario una Fecha de Período de Aporte")]
        public virtual DateTime FechaPeriodo { get; set; } //el periodo de Aporte

        [DisplayName("N° Referencia")]
        public virtual int NumeroReferencia { get; set; }
        [DisplayName("Observación")]
        public virtual string Observacion { get; set; }
        public virtual bool Activo { get; set; }
        public Aporte()
        {
            this.FechaCreacion = DateTime.Now;
            this.Activo = true;
        }

    }
}
