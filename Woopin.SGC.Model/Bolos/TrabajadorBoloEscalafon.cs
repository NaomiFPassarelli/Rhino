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
    public class TrabajadorBoloEscalafon : ISecuredEntity
    {
        public virtual int Id { get; set; }
        //public virtual int NumeroReferencia { get; set; }
        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        [DoNotValidateOnlyId]
        [DisplayName("Escalafón")]
        public virtual Escalafon Escalafon { get; set; }
        [DoNotValidateOnlyId]
        public virtual Trabajador Trabajador { get; set; }
        [DoNotValidateOnlyId]
        public virtual Bolo Bolo { get; set; }
        [DisplayName("Fecha Desde")]
        public virtual DateTime FechaDesde { get; set; }
        [DisplayName("Fecha Hasta")]
        public virtual DateTime FechaHasta { get; set; }
        public TrabajadorBoloEscalafon()
        {
            this.Activo = true;
        }
    }
}
