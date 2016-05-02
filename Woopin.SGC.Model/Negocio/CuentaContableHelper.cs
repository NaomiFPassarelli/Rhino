using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Negocio
{
    public static class CuentaContableHelper
    {
        #region Constructores de Cuentas Contables Basicas
        public static Model.Contabilidad.Cuenta GetCuentaBancoBasica()
        {
            // WARNING: En otros casos Bancos tiene su propio subrubro por lo que deberia ser 2
            // TODO para cuando se agreguen mas niveles al arbol contable
            // Genera la cuenta basica de una cuenta contable de la categoria de bancos
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Corriente = 1,
                Rubro = 1,
                SubRubro = 1
            };
            return cuenta;
        }
        public static Model.Contabilidad.Cuenta GetCuentaCajaBasica()
        {
            // WARNING: En otros casos Bancos tiene su propio subrubro por lo que deberia ser 2
            // Genera la cuenta basica de una cuenta contable de la categoria de bancos
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Corriente = 1,
                Rubro = 1,
                SubRubro = 1
            };
            return cuenta;
        }

        public static Model.Contabilidad.Cuenta GetCuentaRubroBasica(string codigoPadre)
        {
            Model.Contabilidad.Cuenta cuentaPadre = new Model.Contabilidad.Cuenta() { Codigo = codigoPadre };
            cuentaPadre.ParseCodigo();
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Rubro = cuentaPadre.Rubro,
                Corriente = cuentaPadre.Corriente,
                SubRubro = cuentaPadre.SubRubro
            };
            return cuenta;
        }
        public static Contabilidad.Cuenta GetCuentaTarjetaCredito()
        {
            // WARNING: En otros casos Bancos tiene su propio subrubro por lo que deberia ser 2
            // Genera la cuenta basica de una cuenta contable de la categoria de bancos
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Corriente = 1,
                Rubro = 2,
                SubRubro = 4
            };
            return cuenta;
        }

        public static Contabilidad.Cuenta GetCuentaPercepcion()
        {
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Corriente = 1,
                Rubro = 1,
                SubRubro = 3
            };
            return cuenta;
        }
        public static Contabilidad.Cuenta GetCuentaRetencion()
        {
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Corriente = 1,
                Rubro = 1,
                SubRubro = 3
            };
            return cuenta;
        }
        public static Contabilidad.Cuenta GetCuentaRetencionADepositar()
        {
            Model.Contabilidad.Cuenta cuenta = new Model.Contabilidad.Cuenta()
            {
                Corriente = 1,
                Rubro = 2,
                SubRubro = 3
            };
            return cuenta;
        }
        #endregion

        #region Grilla Arbol
        public static int GetLevel(int Numero, int SubRubro, int Corriente, int Rubro)
        {
            if (Rubro > 0 && Corriente == 0 && SubRubro == 0 && Numero == 0)
            {
                return 0;
            }
            else if((Corriente > 0 && SubRubro == 0 && Numero == 0) ||  (Corriente == 0 && SubRubro == 0 && Numero > 0))
            {
                return 1;
            }
            else if ((Corriente > 0 && SubRubro > 0 && Numero == 0) || (Corriente > 0 && SubRubro == 0 && Numero > 0))
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        public static int GetLevel(Contabilidad.Cuenta cuenta)
        {
            return CuentaContableHelper.GetLevel(cuenta.Numero, cuenta.SubRubro, cuenta.Corriente, cuenta.Rubro);
        }
        public static string GetParent(Contabilidad.Cuenta cuenta, IList<Contabilidad.Cuenta> cuentas)
        {

            if (cuenta.Rubro > 0 && cuenta.Corriente == 0 && cuenta.SubRubro == 0 && cuenta.Numero == 0)
            {
                return null;
            }
            else if ((cuenta.Corriente > 0 && cuenta.SubRubro == 0 && cuenta.Numero == 0) || (cuenta.Corriente == 0 && cuenta.SubRubro == 0 && cuenta.Numero > 0))
            {
                return cuentas.Where(x => x.Rubro == cuenta.Rubro && x.Corriente == 0 && x.SubRubro == 0 && x.Numero == 0).SingleOrDefault().Id.ToString();
            }
            else if ((cuenta.Corriente > 0 && cuenta.SubRubro > 0 && cuenta.Numero == 0) || (cuenta.Corriente > 0 && cuenta.SubRubro == 0 && cuenta.Numero > 0))
            {
                return cuentas.Where(x => x.Rubro == cuenta.Rubro && x.Corriente == cuenta.Corriente && x.SubRubro == 0 && x.Numero == 0).SingleOrDefault().Id.ToString();
            }
            else
            {
                return cuentas.Where(x => x.Rubro == cuenta.Rubro && x.Corriente == cuenta.Corriente && x.SubRubro == cuenta.SubRubro && x.Numero == 0).SingleOrDefault().Id.ToString();
            }
        }
        public static string GetParentSumaYSaldo(Contabilidad.SumaSaldo sumaysaldo, IList<Contabilidad.SumaSaldo> sumasysaldos)
        {
            Contabilidad.SumaSaldo ss = new Contabilidad.SumaSaldo();
            if (sumaysaldo.Rubro > 0 && sumaysaldo.Corriente == 0 && sumaysaldo.SubRubro == 0 && sumaysaldo.Numero == 0)
            {
                return null;
            }
            else if ((sumaysaldo.Corriente > 0 && sumaysaldo.SubRubro == 0 && sumaysaldo.Numero == 0) || (sumaysaldo.Corriente == 0 && sumaysaldo.SubRubro == 0 && sumaysaldo.Numero > 0))
            {
                ss = sumasysaldos.Where(x => x.Rubro == sumaysaldo.Rubro && x.Corriente == 0 && x.SubRubro == 0 && x.Numero == 0).SingleOrDefault();
                ss.Saldo += sumaysaldo.Saldo;
                return ss.CuentaId.ToString();
            }
            else if ((sumaysaldo.Corriente > 0 && sumaysaldo.SubRubro > 0 && sumaysaldo.Numero == 0) || (sumaysaldo.Corriente > 0 && sumaysaldo.SubRubro == 0 && sumaysaldo.Numero > 0))
            {
                ss = sumasysaldos.Where(x => x.Rubro == sumaysaldo.Rubro && x.Corriente == sumaysaldo.Corriente && x.SubRubro == 0 && x.Numero == 0).SingleOrDefault();
                ss.Saldo += sumaysaldo.Saldo;
                return ss.CuentaId.ToString();
            }
            else
            {
                ss = sumasysaldos.Where(x => x.Rubro == sumaysaldo.Rubro && x.Corriente == sumaysaldo.Corriente && x.SubRubro == sumaysaldo.SubRubro && x.Numero == 0).SingleOrDefault();
                ss.Saldo += sumaysaldo.Saldo;
                return ss.CuentaId.ToString();
            }
        }

        #endregion


    }
}
