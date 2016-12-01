using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class DireccionMap : ClassMap<Direccion>
    {
        public DireccionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            
        }
    }
}
