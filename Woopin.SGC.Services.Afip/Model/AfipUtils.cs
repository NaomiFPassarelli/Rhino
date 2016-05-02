using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Services.Afip.Model
{
    public static class AfipUtils
    {
        public static int CalcularDigitoVerificadorCB(string input)
        {
            int pares = 0, impares = 0;

            for (var i = 0; i < input.Length; i++)
            {
                if (i % 2 == 0)
                {
                    pares += Convert.ToInt32(input[i]);
                }
                else
                {
                    impares += Convert.ToInt32(input[i]);
                }
            }

            int digitoVerificador = 10 - ((pares + (3 * impares)) % 10);
            return digitoVerificador == 10 ? 0 : digitoVerificador;
        }
    }
}
