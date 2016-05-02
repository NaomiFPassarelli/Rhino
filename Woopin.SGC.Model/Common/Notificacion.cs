using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class Notificacion : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual string Mensaje { get; set; }
        public virtual string Tipo { get; set; }
        public virtual Organizacion Organizacion { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
