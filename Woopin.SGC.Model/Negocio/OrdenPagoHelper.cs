using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.LinqHelpers;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Model.Negocio
{
    public static class OrdenPagoHelper
    {
        public static List<EstadoComprobanteCancelacion> GetEstadosByfilter(CuentaCorrienteFilter filter)
        {
            List<EstadoComprobanteCancelacion> estados = new List<EstadoComprobanteCancelacion>();

            switch (filter)
            {
                case CuentaCorrienteFilter.Todos:
                    foreach (var item in Enum.GetValues(typeof(EstadoComprobanteCancelacion)))
                    {
                        estados.Add((EstadoComprobanteCancelacion)item);
                    }
                    break;
                case CuentaCorrienteFilter.Pendientes:
                case CuentaCorrienteFilter.Vencidos:
                    break;
                default:
                    break;
            }

            return estados;
        }
    }
}
