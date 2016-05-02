using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Contabilidad;
using Woopin.SGC.Repositories.Tesoreria;

namespace Woopin.SGC.Services
{
    public class TesoreriaConfigService : ITesoreriaConfigService
    {
        #region VariablesyConstructor

        private readonly IBancoRepository BancoRepository;
        private readonly ICuentaBancariaRepository CuentaBancariaRepository;
        private readonly ICuentaRepository CuentaRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly ICajaRepository CajaRepository;
        private readonly IValorRepository ValorRepository;
        private readonly ITarjetaCreditoRepository TarjetaRepository;
        private readonly IChequeraRepository ChequeraRepository;
        private readonly IChequePropioRepository ChequePropioRepository;
        public TesoreriaConfigService(IBancoRepository BancoRepository, ICuentaBancariaRepository CuentaBancariaRepository, 
                            IComboItemRepository ComboItemRepository,ITarjetaCreditoRepository TarjetaRepository,
                            ICuentaRepository CuentaRepository, ICajaRepository CajaRepository,
                            IValorRepository ValorRepository, IChequeraRepository ChequeraRepository,
                            IChequePropioRepository ChequePropioRepository)
        {
            this.BancoRepository = BancoRepository;
            this.CuentaBancariaRepository = CuentaBancariaRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.CuentaRepository = CuentaRepository;
            this.CajaRepository = CajaRepository;
            this.ValorRepository = ValorRepository;
            this.TarjetaRepository = TarjetaRepository;
            this.ChequeraRepository = ChequeraRepository;
            this.ChequePropioRepository = ChequePropioRepository;
        }

        #endregion


        #region Banco
        public Banco GetBanco(int Id)
        {
            Banco Banco = null;
            this.BancoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Banco = this.BancoRepository.Get(Id);
            });
            return Banco;
        }

        public IList<Banco> GetAllBancos()
        {
            IList<Banco> Bancos = null;
            this.BancoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Bancos = this.BancoRepository.GetAll();
            });
            return Bancos;
        }
        public void AddBanco(Banco Banco)
        {
            this.BancoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.BancoRepository.Add(Banco);
            });
        }
        public void UpdateBanco(Banco Banco)
        {
            this.BancoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.BancoRepository.Update(Banco);
            });
        }
        public void DeleteBancos(List<int> Ids)
        {
            this.BancoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Banco Banco = this.BancoRepository.Get(Id);
                    Banco.Activo = false;
                    this.BancoRepository.Update(Banco);
                }
            });
        }
        public void CambiarActivo(List<int> Ids, bool Activo)
        {
            this.BancoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach(var Id in Ids)
                {
                    Banco Banco = this.BancoRepository.Get(Id);
                    Banco.Activo = Activo;
                    this.BancoRepository.Update(Banco);
                }
            });
        }

        #endregion

        #region CuentaBancaria
        public CuentaBancaria GetCuentaBancaria(int Id)
        {
            CuentaBancaria CuentaBancaria = null;
            this.CuentaBancariaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CuentaBancaria = this.CuentaBancariaRepository.Get(Id);
            });
            return CuentaBancaria;
        }

        public IList<CuentaBancaria> GetAllCuentasBancarias()
        {
            IList<CuentaBancaria> CuentasBancarias = null;
            this.CuentaBancariaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CuentasBancarias = this.CuentaBancariaRepository.GetAll();
            });
            return CuentasBancarias;
        }

        public IList<CuentaBancaria> GetAllEmiteCheque()
        {
            IList<CuentaBancaria> CuentasBancarias = null;
            this.CuentaBancariaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CuentasBancarias = this.CuentaBancariaRepository.GetAllEmiteCheque();
            });
            return CuentasBancarias;
        }

        public void AddCuentaBancaria(CuentaBancaria CuentaBancaria)
        {
            this.CuentaBancariaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                CuentaBancaria.CuentaContable = CuentaContableHelper.GetCuentaBancoBasica();
                CuentaBancaria.CuentaContable.Nombre = CuentaBancaria.Nombre + " (Nro: " + CuentaBancaria.Numero + ")";
                this.CuentaRepository.Create(CuentaBancaria.CuentaContable);
                this.CuentaBancariaRepository.Add(CuentaBancaria);
            });
        }
        public void UpdateCuentaBancaria(CuentaBancaria CuentaBancaria)
        {
            this.CuentaBancariaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.CuentaBancariaRepository.Update(CuentaBancaria);
            });
        }
        
        public void DeleteCuentasBancarias(List<int> Ids)
        {
            this.CuentaBancariaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    CuentaBancaria CuentaBancaria = this.CuentaBancariaRepository.Get(Id);
                    CuentaBancaria.Activo = false;
                    this.CuentaBancariaRepository.Update(CuentaBancaria);
                }
            });
        }
        public void CambiarActivoCuentaBancaria(List<int> Ids, bool Activo)
        {
            this.CuentaBancariaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    CuentaBancaria CuentaBancaria = this.CuentaBancariaRepository.Get(Id);
                    CuentaBancaria.Activo = Activo;
                    this.CuentaBancariaRepository.Update(CuentaBancaria);
                }
            });
        }

        public SelectCombo GetCuentaBancariaCombos()
        {
            SelectCombo SelectCuentaBancariaCombos = new SelectCombo();
            SelectCuentaBancariaCombos.Items = new List<SelectComboItem>();
            this.CuentaBancariaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                IList<CuentaBancaria> Cuentas = this.CuentaBancariaRepository.GetAll();
                foreach (var Cuenta in Cuentas)
                {
                    SelectComboItem Combo = new SelectComboItem();
                    Combo.id = Cuenta.Id;
                    Combo.text = Cuenta.Banco.Nombre + '(' + Cuenta.Numero + ')';
                    Combo.selected = false;
                    SelectCuentaBancariaCombos.Items.Add(Combo);
                }
            });
            return SelectCuentaBancariaCombos;
        }

        #endregion

        #region TarjetaCredito
        public TarjetaCredito GetTarjetaCredito(int Id)
        {
            TarjetaCredito TarjetaCredito = null;
            this.TarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                TarjetaCredito = this.TarjetaRepository.Get(Id);
            });
            return TarjetaCredito;
        }

        public IList<TarjetaCredito> GetAllTarjetaCreditos()
        {
            IList<TarjetaCredito> TarjetaCreditos = null;
            this.TarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                TarjetaCreditos = this.TarjetaRepository.GetAll();
            });
            return TarjetaCreditos;
        }
        public void AddTarjetaCredito(TarjetaCredito TarjetaCredito)
        {
            this.TarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                TarjetaCredito.CuentaContable = CuentaContableHelper.GetCuentaTarjetaCredito();
                TarjetaCredito.CuentaContable.Nombre = "Deuda Tarjeta de Credito - " + TarjetaCredito.Numero;
                this.CuentaRepository.Create(TarjetaCredito.CuentaContable);

                this.TarjetaRepository.Add(TarjetaCredito);
            });
        }
        public void UpdateTarjetaCredito(TarjetaCredito TarjetaCredito)
        {
            this.TarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.TarjetaRepository.Update(TarjetaCredito);
            });
        }

        public void DeleteTarjetaCreditos(List<int> Ids)
        {
            this.TarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    TarjetaCredito tarjeta = this.TarjetaRepository.Get(Id);
                    tarjeta.Estado = EstadoTarjeta.Cancelada;
                    this.TarjetaRepository.Update(tarjeta);
                }
            });
        }

        public SelectCombo GetTarjetaCreditoCombos()
        {
            SelectCombo SelectTarjetaCombos = new SelectCombo();
            SelectTarjetaCombos.Items = new List<SelectComboItem>();
            this.TarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                IList<TarjetaCredito> Tarjetas = this.TarjetaRepository.GetAll();
                foreach (var tarjeta in Tarjetas)
                {
                    SelectComboItem Combo = new SelectComboItem();
                    Combo.id = tarjeta.Id;
                    Combo.text = tarjeta.Numero + '(' + tarjeta.CuentaBancaria.Nombre + ')';
                    Combo.selected = false;
                    SelectTarjetaCombos.Items.Add(Combo);
                }
            });
            return SelectTarjetaCombos;
        }

        #endregion

        #region Caja
        public Caja GetCaja(int Id)
        {
            Caja Caja = null;
            this.CajaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Caja = this.CajaRepository.Get(Id);
            });
            return Caja;
        }

        public IList<Caja> GetAllCajas()
        {
            IList<Caja> Caja = null;
            this.CajaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Caja = this.CajaRepository.GetAll();
            });
            return Caja;
        }
        public void AddCaja(Caja Caja)
        {
            this.CajaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Caja.CuentaContable = CuentaContableHelper.GetCuentaCajaBasica();
                Caja.CuentaContable.Nombre = Caja.Nombre;
                this.CuentaRepository.Create(Caja.CuentaContable);
                this.CajaRepository.Add(Caja);
            });
        }
        public void UpdateCaja(Caja Caja)
        {
            this.CajaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Cuenta cta = this.CuentaRepository.Get(Caja.CuentaContable.Id);
                cta.Nombre = Caja.Nombre;
                this.CuentaRepository.Update(cta);
                this.CajaRepository.Update(Caja);
            });
        }

        public void DeleteCajas(List<int> Ids)
        {
            this.CajaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Caja Caja = this.CajaRepository.Get(Id);
                    Caja.Activo = false;
                    this.CajaRepository.Update(Caja);
                }
            });
        }

        #endregion


        #region Chequera
        public Chequera GetChequera(int Id)
        {
            Chequera Chequera = null;
            this.ChequeraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Chequera = this.ChequeraRepository.Get(Id);
            });
            return Chequera;
        }

        public IList<Chequera> GetAllChequeras()
        {
            IList<Chequera> Chequera = null;
            this.ChequeraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Chequera = this.ChequeraRepository.GetAll();
            });
            return Chequera;
        }
        public void AddChequera(Chequera Chequera)
        {
            if (Chequera.NumeroDesde > Chequera.NumeroHasta) {
                throw new BusinessException("El Numero Desde debe ser menor que el Hasta");
            }

            this.ChequeraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                IList<Chequera> Chequeras = this.ChequeraRepository.FindChequera(Chequera.CuentaBancaria.Id, Chequera.NumeroDesde, Chequera.NumeroHasta);

                if (Chequeras == null || Chequeras.Count() == 0)
                {
                    this.ChequeraRepository.Add(Chequera);
                }
                else {
                    throw new BusinessException("Ya fue utilizado para esa cuenta bancaria esos numeros de cheques");
                }
                
            });
        }
        public void UpdateChequera(Chequera Chequera)
        {
            this.ChequeraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                IList<Chequera> Chequeras = this.ChequeraRepository.FindChequera(Chequera.CuentaBancaria.Id, Chequera.NumeroDesde, Chequera.NumeroHasta);

                if (Chequeras == null || Chequeras.Count() == 0 || (Chequeras.Count() == 1 && Chequeras.First().Id == Chequera.Id))
                {
                    Chequera cra = this.ChequeraRepository.Get(Chequera.Id);
                    cra.CuentaBancaria = Chequera.CuentaBancaria;
                    cra.Nombre = Chequera.Nombre;
                    cra.NumeroDesde = Chequera.NumeroDesde;
                    cra.NumeroHasta = Chequera.NumeroHasta;
                    this.ChequeraRepository.Update(cra);
                }
                else
                {
                    throw new BusinessException("Ya fue utilizado para esa cuenta bancaria esos numeros de cheques");
                }
            });
        }

        public void DeleteChequeras(List<int> Ids)
        {
            this.ChequeraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Chequera Chequera = this.ChequeraRepository.Get(Id);
                    this.ChequeraRepository.Delete(Chequera);
                }
            });
        }

        public IList<ChequePropio> GetAllChequesInChequera(int IdChequera)
        {
            IList<ChequePropio> ChequesPropio = null;
            this.ChequeraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Chequera Chequera = this.ChequeraRepository.Get(IdChequera);
                ChequesPropio = this.ChequePropioRepository.GetAllInChequera(Chequera.CuentaBancaria.Id, Chequera.NumeroDesde, Chequera.NumeroHasta);
            });
            return ChequesPropio;
        }


        public void ControlChequePropioChequera(int IdCuentaBancaria, int Numero)
        {
            this.ChequeraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Chequera cra = this.ChequeraRepository.ControlChequePropioChequera(IdCuentaBancaria, Numero);
                if (cra == null)
                    throw new BusinessException("No pertenece a ninguna Chequera o no existe Chequera en esta Cuenta, debe ir a Tesoriaria>Archivos>Chequeras");

            });
        }

        #endregion


        #region Valor
        public Valor GetValor(int Id)
        {
            Valor Valor = null;
            this.ValorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Valor = this.ValorRepository.Get(Id);
            });
            return Valor;
        }

        public IList<Valor> GetAllValores()
        {
            IList<Valor> Valores = null;
            this.ValorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Valores = this.ValorRepository.GetAll();
            });
            return Valores;
        }
        public void AddValor(Valor Valor)
        {
            this.ValorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddValorNT(Valor);
            });
        }

        public void AddValorNT(Valor Valor)
        {
            this.ValorRepository.Add(Valor);
        }

        public void UpdateValor(Valor Valor)
        {
            this.ValorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ValorRepository.Update(Valor);
            });
        }
        public void DeleteValores(List<int> Ids)
        {
            this.ValorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Valor Valor = this.ValorRepository.Get(Id);
                    Valor.Activo = false;
                    this.ValorRepository.Update(Valor);
                }
            });
        }

        #endregion


    }
}
