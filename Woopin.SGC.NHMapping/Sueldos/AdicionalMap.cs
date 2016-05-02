﻿using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.NHMapping.Sueldos
{
    public class AdicionalMap : ClassMap<Adicional>
    {
        public AdicionalMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Porcentaje).Nullable().Not.LazyLoad();
            this.Map(c => c.Valor).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Suma).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TipoLiquidacion).Not.Nullable().Not.LazyLoad();
        }
    }
}