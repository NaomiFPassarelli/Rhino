using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Contabilidad
{
    public class BloqueoContable : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime Inicio { get; set; }
        public virtual DateTime Final { get; set; }
        public virtual string Nombre { get; set; }
        public virtual Ejercicio Ejercicio { get; set; }
        public virtual bool Activo { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public BloqueoContable()
        {
            this.Activo = true;
        }
    }
}
