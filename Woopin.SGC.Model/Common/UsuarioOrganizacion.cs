using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class UsuarioOrganizacion
    {
        public virtual int Id { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Organizacion Organizacion { get; set; }
    }
}
