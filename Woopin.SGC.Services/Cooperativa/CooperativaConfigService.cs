using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Ventas;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Repositories.Cooperativa;
using Woopin.SGC.Model.Cooperativa;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    
    public class CooperativaConfigService : ICooperativaConfigService
    {

        #region VariablesyConstructor

        private readonly IAsociadoRepository AsociadoRepository;
        private readonly IComboItemRepository ComboItemRepository;

        public CooperativaConfigService(IAsociadoRepository AsociadoRepository, IComboItemRepository ComboItemRepository)
        {
            this.AsociadoRepository = AsociadoRepository;
            this.ComboItemRepository = ComboItemRepository;
        }

        #endregion

        #region Asociado
        public Asociado GetAsociado(int Id)
        {
            Asociado Asociado = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociado = this.AsociadoRepository.Get(Id);
            });
            return Asociado;
        }

        public Asociado GetAsociadoCompleto(int Id)
        {
            Asociado Asociado = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociado = this.AsociadoRepository.GetCompleto(Id);
            });
            return Asociado;
        }

        public IList<Asociado> GetAllAsociados()
        {
            IList<Asociado> Asociados = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.AsociadoRepository.GetAll();
            });
            return Asociados;
        }

        [Loggable]
        public void AddAsociado(Asociado Asociado)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Asociado.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Asociado.CUIT, null))
                    throw new BusinessException("El CUIT ya existe");

                Asociado.CantidadCuotasAbonadas = 0;
                this.AddAsociadoNT(Asociado); 
            });
        }
        public void AddAsociadoNT(Asociado Asociado)
        {
            this.AsociadoRepository.Add(Asociado);
        }

        [Loggable]
        public void UpdateAsociado(Asociado Asociado)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Asociado.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Asociado.CUIT, Asociado.Id))
                    throw new BusinessException("El CUIT ya existe");


                Asociado ToUpdate = this.AsociadoRepository.Get(Asociado.Id);
                ToUpdate.Apellido = Asociado.Apellido;
                ToUpdate.CUIT = Asociado.CUIT;
                ToUpdate.DNI = Asociado.DNI;
                ToUpdate.Nombre = Asociado.Nombre;
                ToUpdate.FechaIngreso = Asociado.FechaIngreso;
                ToUpdate.FechaEgreso = Asociado.FechaEgreso;
                ToUpdate.FechaActaIngreso = Asociado.FechaActaIngreso;
                ToUpdate.ActaAlta = Asociado.ActaAlta;
                ToUpdate.ActaBaja = Asociado.ActaBaja;
                ToUpdate.CantidadCuotas = Asociado.CantidadCuotas;
                ToUpdate.ImporteCuota = Asociado.ImporteCuota;
                this.AsociadoRepository.Update(ToUpdate);
            });
        }
        public void DeleteAsociados(List<int> Ids)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Asociado Asociado = this.AsociadoRepository.Get(Id);
                    Asociado.Activo = false;
                    this.AsociadoRepository.Update(Asociado);
                }
            });
        }

        public SelectCombo GetAsociadoCombos()
        {
            SelectCombo SelectAsociadoCombos = new SelectCombo();
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectAsociadoCombos.Items = this.AsociadoRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido
                                                              }).ToList();
            });
            return SelectAsociadoCombos;
        }

        public SelectCombo GetAllAsociadosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectAsociadoCombos = new SelectCombo();
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectAsociadoCombos.Items = this.AsociadoRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido + " " + "(" + x.CUIT + ")"
                                                              }).ToList();
            });
            return SelectAsociadoCombos;
        }

        [Loggable]
        public void ImportAsociados(List<Asociado> Asociados)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Asociado in Asociados)
                {
                    this.ImportAsociado(Asociado);
                }
            });
        }

        [Loggable]
        public void ImportAsociado(Asociado Asociado)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ImportAsociadoNT(Asociado);
            });
        }

        [Loggable]
        public void ImportAsociadoNT(Asociado Asociado)
        {
            if (!ComprobanteHelper.IsCUITValid(Asociado.CUIT))
                throw new BusinessException("El CUIT es invalido.");

            if (this.ExistCUITNT(Asociado.CUIT, null))
                throw new BusinessException("El CUIT coincide con uno ya creado");

            this.AsociadoRepository.Add(Asociado);
        }

        public bool ExistCUITNT(string cuit, int? IdUpdate)
        {
            return this.AsociadoRepository.ExistCUIT(cuit, IdUpdate);
        }

        public IList<Asociado> GetAsociadosMes(int Mes, int Año)
        {
            return this.AsociadoRepository.GetAsociadosMes(Mes, Año);
        }
        

        #endregion

    }
}
