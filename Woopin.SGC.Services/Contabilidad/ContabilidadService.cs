using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Compras;
using Woopin.SGC.Repositories.Contabilidad;
using Woopin.SGC.Repositories.Ventas;
using Woopin.SGC.Common.App.Logging;

namespace Woopin.SGC.Services
{
    public class ContabilidadService : IContabilidadService
    {
        #region VariablesyConstructor
        private readonly ICuentaRepository cuentaRepository;
        private readonly IClienteRepository clienteRepository;
        private readonly IAsientoRepository asientoRepository;
        private readonly IAsientoItemRepository asientoItemRepository;
        private readonly IProveedorRepository proveedorRepository;
        private readonly IRubroCompraRepository rubroRepository;
        private readonly IEjercicioRepository EjercicioRepository;
        private readonly IUsuarioRepository UsuarioRepository;
        private AsientosHelper asientosHelper { get; set; }
        public ContabilidadService(ICuentaRepository cuentaRepository, IClienteRepository clienteRepository, IAsientoRepository asientoRepository, IAsientoItemRepository asientoItemRepository,
            IRubroCompraRepository rubroRepository, IProveedorRepository proveedorRepository, IEjercicioRepository EjercicioRepository, IUsuarioRepository UsuarioRepository)
        {
            this.cuentaRepository = cuentaRepository;
            this.clienteRepository = clienteRepository;
            this.asientoRepository = asientoRepository;
            this.asientoItemRepository = asientoItemRepository;
            this.rubroRepository = rubroRepository;
            this.proveedorRepository = proveedorRepository;
            this.asientosHelper = new AsientosHelper();
            this.EjercicioRepository = EjercicioRepository;
            this.UsuarioRepository = UsuarioRepository;
        }
        #endregion

        #region Asientos de Operaciones
        public Asiento NuevoAsientoVentaNT(ComprobanteVenta Comprobante)
        {
            Asiento asiento = this.asientosHelper.AsientoComprobanteVenta(Comprobante);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoCompraNT(ComprobanteCompra Comprobante)
        {
            Proveedor proveedor = this.proveedorRepository.Get(Comprobante.Proveedor.Id);
            foreach (var item in Comprobante.Detalle)
            {
                item.RubroCompra = this.rubroRepository.Get(item.RubroCompra.Id);
            }
            Asiento asiento = this.asientosHelper.AsientoComprobanteCompra(Comprobante, proveedor);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoMovimientoFondos(MovimientoFondo movimiento)
        {
            Asiento asiento = this.asientosHelper.AsientoMovimientoFondos(movimiento);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoCanjeCheques(ChequePropio newCheque, ChequePropio chequeAnterior)
        {
            Asiento asiento = this.asientosHelper.AsientoCanjeCheque(newCheque, chequeAnterior);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoOP(OrdenPago op)
        {
            Asiento asiento = this.asientosHelper.AsientoOrdenPago(op);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoCobranza(Cobranza cobranza)
        {
            Asiento asiento = this.asientosHelper.AsientoCobranza(cobranza);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoDeposito(Deposito deposito)
        {
            Asiento asiento = this.asientosHelper.AsientoDeposito(deposito);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoCancelacionTarjeta(CancelacionTarjeta cancelacion)
        {
            Asiento asiento = this.asientosHelper.AsientoCancelacionTarjeta(cancelacion);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }

        public Asiento NuevoAsientoOE(OtroEgreso oe)
        {
            foreach (var item in oe.Detalle)
            {
                item.RubroCompra = this.rubroRepository.Get(item.RubroCompra.Id);
            }
            Asiento asiento = this.asientosHelper.AsientoOtroEgreso(oe);
            asiento.Manualizado = false;
            return this.BuildAndSaveAsiento(asiento);
        }
        #endregion

        #region Asientos

        [Loggable]
        public void AddAsiento(Asiento asiento)
        {
            this.cuentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach(var item in asiento.Items)
                {
                    item.Asiento = asiento;
                }
                asiento.Manualizado = true;
                this.BuildAndSaveAsiento(asiento);
            });
        }

        public void UpdateAsiento(Asiento Asiento)
        {
            this.asientoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.UpdateAsientoNT(Asiento);
            });
        }

        public void UpdateAsientoNT(Asiento Asiento)
        {
            this.asientoRepository.Update(Asiento);
        }

        public int GetProximoIdAsiento()
        {
            int NextId = 0;
            this.asientoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                NextId = this.asientoRepository.GetProximoIdAsiento();
            });
            return NextId;
        }

        public Asiento GetAsientoCompleto(int Id)
        {
            Asiento asiento = null;
            this.asientoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                asiento = this.asientoRepository.GetCompleto(Id);
            });
            return asiento;
        }

        [Loggable]
        public Asiento BuildAndSaveAsiento(Asiento asiento)
        {
            foreach (var item in asiento.Items.Where(x => x.Cuenta.Id == 0).ToList())
            {
                if (item.Cuenta.Codigo == null)
                {
                    throw new BusinessException("La cuenta contable no posee el codigo para completar la operación.");
                }
                item.Cuenta.Id = this.cuentaRepository.GetCuentaByCodigo(item.Cuenta.Codigo).Id;
            }
            if(asiento.Ejercicio == null || asiento.Ejercicio.Id == 0)
            {
                asiento.Ejercicio = this.EjercicioRepository.GetByDate(asiento.Fecha);
            }
            asiento.Usuario = Security.GetCurrentUser();
            this.asientoRepository.Add(asiento);
            foreach (var item in asiento.Items)
            {
                this.asientoItemRepository.Add(item);
            }
            return asiento;
        }

        public IList<Asiento> GetAsientosFilter( DateTime? start, DateTime? end)
        {
            IList<Asiento> ret = null;
            this.asientoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.asientoRepository.GetAsientosFilter( _start, _end);
                
            });
            return ret;
        }

        public LibroMayorHeader GetAsientosHeaderFilterCuenta(int id, DateTime? start, DateTime? end)
        {
            LibroMayorHeader ret = null;
            this.asientoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.asientoRepository.GetAsientosHeaderFilterCuenta(id, _start, _end);
            });
            return ret;
        }

        public IList<LibroMayor> GetAsientosFilterCuenta(int id, DateTime? start, DateTime? end)
        {
            IList<LibroMayor> ret = null;
            this.asientoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                
                ret = this.asientoRepository.GetAsientosFilterCuenta(id, _start, _end);

            });
            return ret;
        }
        #endregion
        
        public bool ControlarIngreso(DateTime fechaContable)
        {
            bool controlIngreso = false;
            this.EjercicioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                controlIngreso = this.ControlarIngresoNT(fechaContable);
            });
            return controlIngreso;
        }
        
        public bool ControlarIngresoNT(DateTime fechaContable)
        {
            try
            {
                this.EjercicioRepository.ControlarIngreso(fechaContable);
                return true;
            }
            catch (ValidationException)
            {
                return false;
            }
        }

        public void TryControlarIngresoNT(DateTime fechaContable)
        {
            this.EjercicioRepository.ControlarIngreso(fechaContable);
        }

        public void DeleteAsiento(int Id)
        {
            this.asientoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DeleteAsientoNT(Id);
            });
        }

        public void DeleteAsientoNT(int Id)
        {
            Asiento a = this.asientoRepository.Get(Id);
            this.asientoRepository.Delete(a);
        }

    }
}
