using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class Combo
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public virtual string Nombre { get; set; }
        public virtual bool Activo { get; set; }
        public Combo()
        {
            this.Activo = true;
        }

    }


    public enum ComboType
    {
        LetraComprobante = 1,
        //CondicionCompra = 2,  // Disponible!
        MovimientoFondo = 3,
        Paises = 4,
        TipoComprobanteVenta = 5,
        CondicionCompraVenta = 6,
        TipoValor = 7,
        TipoComprobanteCompra = 8, 
        TipoIva = 9,
        TipoCobranza = 10,
        TipoOrdenPago = 11,
        ActividadOrganizacion = 12,
        TipoIVAOrganizacion = 13,
        UnidadesMedidas = 14,
        CategoriasEmpleados = 15,
        TareasEmpleados = 16,
        Sexo = 17,
        EstadoCivil = 18,
        Sindicato = 19,
        ObraSocial = 20,
        BancoDeposito = 21
    }
}
