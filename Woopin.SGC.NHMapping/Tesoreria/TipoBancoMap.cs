using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.NHMapping
{
    public class TipoBancoMap : ClassMap<TipoBanco>
    {
        public TipoBancoMap()
        {
            this.Id(c => c.Id).Column("IdTipoBanco").GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
        }
    }
}
