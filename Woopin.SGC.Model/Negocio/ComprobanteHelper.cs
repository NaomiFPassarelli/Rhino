using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Negocio
{
    public static class ComprobanteHelper
    {
        public const string PrimerComprobante = "0001-00000001";
        public const string PrimerFEComprobante = "0002-00000001";
        public static List<EstadoComprobante> GetEstadosByfilter(CuentaCorrienteFilter filter)
        {
            List<EstadoComprobante> estados = new List<EstadoComprobante>();

            switch (filter)
            {
                case CuentaCorrienteFilter.Todos:
                    foreach (var item in Enum.GetValues(typeof(EstadoComprobante)))
                    {
                        estados.Add((EstadoComprobante)item);
                    }
                    break;
                case CuentaCorrienteFilter.Pendientes:
                    estados.Add(EstadoComprobante.Creada);
                    break;
                case CuentaCorrienteFilter.Vencidos:
                    estados.Add(EstadoComprobante.Creada);
                    estados.Add(EstadoComprobante.Vencida);
                    break;
                case CuentaCorrienteFilter.Anulados:
                    estados.Add(EstadoComprobante.Anulada);
                    break;
                case CuentaCorrienteFilter.Pendiente_Afip:
                    estados.Add(EstadoComprobante.Pendiente_Afip);
                    break;
                default:
                    break;
            }

            return estados;
        }


        public static string GetProximoComprobante(string ultimoComprobante)
        {
            string[] talonarioNumero = ultimoComprobante.Split('-');
            if (int.Parse(talonarioNumero[1]) <= 99999999)
            {
                talonarioNumero[1] = (int.Parse(talonarioNumero[1]) + 1).ToString("00000000");
            }
            else
            {
                talonarioNumero[0] = (int.Parse(talonarioNumero[0]) + 1).ToString("0000");
                talonarioNumero[1] = "00000001";
            }
            return talonarioNumero[0] + '-' + talonarioNumero[1];
        }


        public static bool IsCUITValid(string cuit)
        {
            string[] strCuit = cuit.Split('-');
            int[] arrCuit = cuit.Replace("-",string.Empty).Substring(0,10).Select(x => Convert.ToInt32(x.ToString())).ToArray();
            int[] arrCorrector = new int[] { 5,4,3,2,7,6,5,4,3,2 };
            int CodigoVerificadorAControlar = Convert.ToInt32(cuit.Substring(cuit.Length - 1,1));
            int CodigoVerificador = 0;
            int total = 0;
            for(var i = 0; i < 10; i++)
            {
                total += arrCorrector[i] * arrCuit[i];
            }
            int resto = total % 11;
            if(resto == 0)
            {
                CodigoVerificador = 0;
            }
            else if (resto == 1)
            {
                if (strCuit[0] == "20")
                {
                    CodigoVerificador = 9;
                }
                else{
                    CodigoVerificador = 4;
                }
            }
            else
            {
                CodigoVerificador = (11 - resto);
            }

            return CodigoVerificador == CodigoVerificadorAControlar;
        }

    }
}
