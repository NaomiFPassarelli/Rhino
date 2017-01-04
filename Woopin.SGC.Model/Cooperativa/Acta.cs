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
    public class Acta : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [DoNotValidateOnlyId]
        public virtual IList<Asociado> AsociadosIngreso { get; set; }

        [DoNotValidateOnlyId]
        public virtual IList<Asociado> AsociadosEgreso { get; set; }
        public virtual IList<ActaPuntoExtra> OtrosPuntos { get; set; }
        
        [Required(ErrorMessage = "Es Necesario una fecha de acta")]
        [DisplayName("Fecha Acta")]
        public virtual DateTime FechaActa { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public virtual string Presidente { get; set; }
        public virtual string Secretario { get; set; }
        [DisplayName("Otro Presente")]
        public virtual string OtroPresente { get; set; }
        public virtual string Tesorero { get; set; }

        [DisplayName("Fecha Finalizacion Acta")]
        public virtual DateTime? FechaFinalizacionActa { get; set; } // de la reunion de alta
        [DisplayName("Numero Acta")]
        public virtual int NumeroActa { get; set; }
        [Required(ErrorMessage = "Es Necesario una fecha de creación")]
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }
        
        public Acta()
        {
            this.FechaCreacion = DateTime.Now;
        }

    }
}
