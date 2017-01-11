using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class OrganizacionModulo
    {
        public virtual int Id { get; set; }
        public virtual ModulosSistemaGestion ModulosSistemaGestion { get; set; }
        public virtual Organizacion Organizacion { get; set; }
    }

    public enum ModulosSistemaGestion
    {
        Venta = 1,
        Compra = 2, 
        Tesoreria = 3,
        Contabilidad = 4,
        Stock = 5, 
        Sueldos = 6,
        Cooperativa = 7, 
        Configuracion = 8,
        Management = 9
    }

}
