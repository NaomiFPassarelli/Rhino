using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Model.Negocio
{
    public static class ValoresHelper
    {
        // Movimientos de fondos
        public const int Deposito = 30;
        public const int Extraccion = 31;
        public const int Transferencia = 32;

        public static List<EstadoCheque> GetEstadosChequesByfilter(FilterCheque filter)
        {
            List<EstadoCheque> estados = new List<EstadoCheque>();

            switch (filter)
            {
                case FilterCheque.Todos:
                    estados.Add(EstadoCheque.Depositado);
                    estados.Add(EstadoCheque.Entregado);
                    estados.Add(EstadoCheque.Anulado);
                    estados.Add(EstadoCheque.Cartera);
                    estados.Add(EstadoCheque.Devuelto);
                    estados.Add(EstadoCheque.Pagado);
                    break;
                case FilterCheque.Depositados:
                    estados.Add(EstadoCheque.Depositado);
                    break;
                case FilterCheque.Entregados:
                    estados.Add(EstadoCheque.Entregado);
                    break;
                case FilterCheque.Anulados:
                    estados.Add(EstadoCheque.Anulado);
                    break;
                case FilterCheque.Cartera:
                    estados.Add(EstadoCheque.Cartera);
                    break;
                case FilterCheque.Devueltos:
                    estados.Add(EstadoCheque.Devuelto);
                    break;
                case FilterCheque.Pagados:
                    estados.Add(EstadoCheque.Pagado);
                    break;
                default:
                    break;
            }

            return estados;
        }
    }
}
