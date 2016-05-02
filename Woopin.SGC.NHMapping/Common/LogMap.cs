using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping.Common
{
    public class LogMap : ClassMap<Log>
    {
        public LogMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Date).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Logger).Nullable().Not.LazyLoad();
            this.Map(c => c.Level).Nullable().Not.LazyLoad();
            this.Map(c => c.Message).Not.Nullable().Length(4000).Not.LazyLoad();
            this.Map(c => c.Thread).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Exception).Nullable().Length(2000).Not.LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Nullable().LazyLoad();
        }
    }
}
