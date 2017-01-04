using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class Cheque : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Banco")]
        [DoNotValidateOnlyId]
        public virtual Banco Banco { get; set; }

        [Required(ErrorMessage = "Es Necesario un Numero")]
        [DisplayName("Número")]
        public virtual string Numero { get; set; }

        // TODO - PIROSKA - Se saca el requerido para piroska.
        //[Required(ErrorMessage = "Es Necesario un Numero de Cta")]
        [DisplayName("Número Cuenta")]
        public virtual string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "Es Necesario un Importe")]
        [DisplayName("Monto")]
        public virtual decimal Importe { get; set; }

        [DisplayName("Fecha Emisión")]
        public virtual DateTime Fecha { get; set; }
        
        [DisplayName("Vencimiento")]
        public virtual DateTime FechaVencimiento { get; set; }

        [DisplayName("Fecha Efectivizado")]
        public virtual DateTime? FechaEfectivizado { get; set; }

        [Required(ErrorMessage = "Es Necesario una fecha de creación")]
        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }
        public virtual EstadoCheque Estado { get; set; }

        [DoNotValidateOnlyId]
        public virtual Cliente Cliente { get; set; }
        public virtual bool Propio { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
    }


    /*
     * 
     * Tener en cuenta que la numeracion se usa para los filtros en los listados de cheques propios y cheques.
     * 
     */
    public enum EstadoCheque
    {
        Borrador = 0,
        Cartera = 1,
        Entregado = 2,
        Depositado = 3,
        Devuelto = 4,
        Anulado = 5,
        Pagado = 6
    }

    public enum FilterCheque
    {
        Todos = 0,
        Cartera = 1,
        Entregados = 2,
        Depositados = 3,
        Devueltos = 4,
        Anulados = 5,
        Pagados = 6
    }
}
