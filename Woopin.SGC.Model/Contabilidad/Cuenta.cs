using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Contabilidad
{
    public class Cuenta : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual string Codigo { get; set; }
        public virtual int Rubro { get; set; }
        public virtual int Corriente { get; set; }
        public virtual int SubRubro { get; set; }
        public virtual int Numero { get; set; }

        [Required(ErrorMessage="Es Necesario el Nombre")]
        public virtual string Nombre { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Cuenta()
        {

        }

        public Cuenta(string codigo, string nombre)
        {
            this.Codigo = codigo;
            this.Nombre = nombre;
        }

        public virtual void CalcularCodigo()
        {
            bool esHoja = this.Numero > 0;
            if (esHoja)
            {
                if (Corriente == 0)
                {
                    this.Codigo = Rubro.ToString() + "." + (Numero).ToString("000");
                }
                else if (SubRubro == 0)
                {
                    this.Codigo = Rubro.ToString() + "." + Corriente.ToString() + "." + (Numero).ToString("000");
                }
                else
                {
                    this.Codigo = Rubro.ToString() + "." + Corriente.ToString() + "." + SubRubro.ToString() + "." + (Numero).ToString("000");
                }
            }
            else
            {
                this.Codigo = Rubro.ToString() + "." + Corriente.ToString() + "." + SubRubro.ToString() + "." + (Numero).ToString("000");
            }
        }

        public virtual string CodigoProxima()
        {
            bool esHoja = this.Numero > 0;
            int proximoNro = this.Numero + 1;
            if (esHoja)
            {
                if (Corriente == 0)
                {
                    return Rubro.ToString() + "." + (proximoNro).ToString("000");
                }
                else if (SubRubro == 0)
                {
                    return Rubro.ToString() + "." + Corriente.ToString() + "." + (proximoNro).ToString("000");
                }
                else
                {
                    return  Rubro.ToString() + "." + Corriente.ToString() + "." + SubRubro.ToString() + "." + (proximoNro).ToString("000");
                }
            }
            else
            {
                if (SubRubro == 0)
                {
                    return Rubro.ToString() + "." + (Corriente + 1) + ".0.0" ;
                }
                else if (Numero == 0)
                {
                    return Rubro.ToString() + "." + Corriente.ToString() + "." + (SubRubro + 1) + ".0";
                }
                else
                {
                    return Rubro.ToString() + "." + Corriente.ToString() + "." + SubRubro.ToString() + "." + (proximoNro).ToString("000");
                }
            }
        }

        public virtual string CodigoPrimerHijo()
        {
            bool esHoja = this.Numero > 0;
            if (esHoja)
            {
                return null;
            }
            else
            {
                if (Corriente == 0)
                {
                    return Rubro.ToString() + "." + "001";
                }
                else
                {
                    return Rubro.ToString() + "." + Corriente.ToString() + "." + "001";
                }
            }
        }

        public virtual void ParseCodigo()
        {
            // Se genero desde codigo.
            string[] nros = this.Codigo.Split('.');
            this.Rubro = Convert.ToInt32(nros[0]);
            this.Corriente = nros.Length > 2 ? Convert.ToInt32(nros[1]) : 0;
            this.SubRubro = nros.Length > 3 ? Convert.ToInt32(nros[2]) : 0;
            if (nros.Length == 2) this.Numero = Convert.ToInt32(nros[1]);
            else if (nros.Length == 3) this.Numero = Convert.ToInt32(nros[2]);
            else this.Numero = Convert.ToInt32(nros[3]);
        }
    }
}
