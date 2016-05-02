using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Ventas
{
    public class GrupoEconomico : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual string Nombre { get; set; }
        public virtual bool Activo { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public GrupoEconomico()
        {
            this.Activo = true;
        }
    }
}
