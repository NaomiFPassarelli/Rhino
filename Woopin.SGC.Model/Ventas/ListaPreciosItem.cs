using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Model.Ventas
{
    public class ListaPreciosItem : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual decimal Precio { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Articulo Articulo { get; set; }
        public virtual GrupoEconomico Grupo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }

    public static class TipoListaPrecios
    {
        public const string Cliente = "C";
        public const string Grupo = "G";
        public const string Default = "D";
    }
}
