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
using Woopin.SGC.Repositories.Stock;
using Woopin.SGC.Model.Stock;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    
    public class StockConfigService : IStockConfigService
    {

        #region VariablesyConstructor

        private readonly IArticuloRepository ArticuloRepository;
        private readonly IRubroArticuloRepository RubroArticuloRepository;
        private readonly IListaPreciosRepository ListaPreciosRepository;
        private readonly IIngresoStockRepository IngresoStockRepository;
        public StockConfigService(IArticuloRepository ArticuloRepository, IRubroArticuloRepository RubroArticuloRepository,
                                IListaPreciosRepository ListaPreciosRepository, IIngresoStockRepository IngresoStockRepository)
        {
            this.ArticuloRepository = ArticuloRepository;
            this.RubroArticuloRepository = RubroArticuloRepository;
            this.ListaPreciosRepository = ListaPreciosRepository;
            this.IngresoStockRepository = IngresoStockRepository;
        }

        #endregion

        #region Articulo
        public Articulo GetArticulo(int Id)
        {
            Articulo Articulo = null;
            this.ArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Articulo = this.ArticuloRepository.Get(Id);
            });
            return Articulo;
        }

        public Articulo GetArticuloConStock(int Id)
        {
            Articulo Articulo = null;
            this.ArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Articulo = this.ArticuloRepository.GetConStock(Id);
            });
            return Articulo;
        }

        public IList<Articulo> GetAllArticulos()
        {
            IList<Articulo> Articulos = null;
            this.ArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Articulos = this.ArticuloRepository.GetAll();
            });
            return Articulos;
        }

        [Loggable]
        public void AddArticulo(Articulo Articulo)
        {
            this.ArticuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (Articulo.Inventario && Articulo.Tipo == TipoArticulo.Producto && (Articulo.Stock == null || Articulo.Stock < 0))
                {
                    Articulo.Stock = 0;
                }
                this.AddArticuloNT(Articulo); 
                if (Articulo.Inventario && Articulo.Tipo == TipoArticulo.Producto && Articulo.Stock > 0)
                {
                    IngresoStock IS = new IngresoStock();
                    IS.Articulo = Articulo;
                    IS.Cantidad = (decimal)Articulo.Stock;
                    IS.FechaCreacion = DateTime.Now;
                    IS.Observacion = "Creacion de Articulo - Stock Inicial";
                    this.IngresoStockRepository.Add(IS);
                }
            });
        }
        public void AddArticuloNT(Articulo Articulo)
        {
            this.ArticuloRepository.Add(Articulo);
            ListaPreciosItem itemLista = new ListaPreciosItem()
            {
                Articulo = Articulo,
                Precio = 0
            };
            this.ListaPreciosRepository.Add(itemLista);
        }

        [Loggable]
        public void UpdateArticulo(Articulo Articulo)
        {
            this.ArticuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Articulo ToUpdate = this.ArticuloRepository.Get(Articulo.Id);
                ToUpdate.Descripcion = Articulo.Descripcion;
                ToUpdate.CodigoBarras = Articulo.CodigoBarras;
                ToUpdate.Rubro = Articulo.Rubro;
                ToUpdate.AlicuotaIVA = Articulo.AlicuotaIVA;
                ToUpdate.UnidadMedida = Articulo.UnidadMedida;
                ToUpdate.Tipo = Articulo.Tipo;
                ToUpdate.Estado = Articulo.Estado;
                ToUpdate.Inventario = Articulo.Inventario; //Stock no editable
                this.ArticuloRepository.Update(ToUpdate);
            });
        }
        public void DeleteArticulos(List<int> Ids)
        {
            this.ArticuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Articulo Articulo = this.ArticuloRepository.Get(Id);
                    Articulo.Activo = false;
                    this.ArticuloRepository.Update(Articulo);
                }
            });
        }

        public SelectCombo GetArticuloCombos()
        {
            SelectCombo SelectArticuloCombos = new SelectCombo();
            this.ArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectArticuloCombos.Items = this.ArticuloRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectArticuloCombos;
        }

        public SelectCombo GetAllArticulosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectArticuloCombos = new SelectCombo();
            this.ArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectArticuloCombos.Items = this.ArticuloRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectArticuloCombos;
        }

        public SelectCombo GetAllArticulosConStockByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectArticuloCombos = new SelectCombo();
            this.ArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectArticuloCombos.Items = this.ArticuloRepository.GetAllConStockByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion + '(' + x.UnidadMedida.Data + ')'
                                                              }).ToList();
            });
            return SelectArticuloCombos;
        }

        #endregion

        #region RubroArticulo
        public RubroArticulo GetRubroArticulo(int Id)
        {
            RubroArticulo RubroArticulo = null;
            this.RubroArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                RubroArticulo = this.RubroArticuloRepository.Get(Id);
            });
            return RubroArticulo;
        }

        public IList<RubroArticulo> GetAllRubroArticulos()
        {
            IList<RubroArticulo> RubroArticulos = null;
            this.RubroArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                RubroArticulos = this.RubroArticuloRepository.GetAll();
            });
            return RubroArticulos;
        }

        [Loggable]
        public void AddRubroArticulo(RubroArticulo RubroArticulo)
        {
            this.RubroArticuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddRubroArticuloNT(RubroArticulo);
            });
        }
        public void AddRubroArticuloNT(RubroArticulo RubroArticulo)
        {
            this.RubroArticuloRepository.Add(RubroArticulo);
        }

        [Loggable]
        public void UpdateRubroArticulo(RubroArticulo RubroArticulo)
        {
            this.RubroArticuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                RubroArticulo ToUpdate = this.RubroArticuloRepository.Get(RubroArticulo.Id);
                ToUpdate.Descripcion = RubroArticulo.Descripcion;
                this.RubroArticuloRepository.Update(ToUpdate);
            });
        }
        public void DeleteRubroArticulos(List<int> Ids)
        {
            this.RubroArticuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    RubroArticulo RubroArticulo = this.RubroArticuloRepository.Get(Id);
                    RubroArticulo.Activo = false;
                    this.RubroArticuloRepository.Update(RubroArticulo);
                }
            });
        }

        public SelectCombo GetRubroArticuloCombos()
        {
            SelectCombo SelectRubroArticuloCombos = new SelectCombo();
            this.RubroArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectRubroArticuloCombos.Items = this.RubroArticuloRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectRubroArticuloCombos;
        }

        public SelectCombo GetAllRubroArticulosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectRubroArticuloCombos = new SelectCombo();
            this.RubroArticuloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectRubroArticuloCombos.Items = this.RubroArticuloRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectRubroArticuloCombos;
        }

        #endregion
    }
}
