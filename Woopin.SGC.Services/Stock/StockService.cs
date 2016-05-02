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
    
    public class StockService : IStockService
    {

        #region VariablesyConstructor

        private readonly IIngresoStockRepository IngresoStockRepository;
        private readonly IEgresoStockRepository EgresoStockRepository;
        private readonly IArticuloRepository ArticuloRepository;

        public StockService(IIngresoStockRepository IngresoStockRepository, IEgresoStockRepository EgresoStockRepository,
                                IArticuloRepository ArticuloRepository)
        {
            this.IngresoStockRepository = IngresoStockRepository;
            this.EgresoStockRepository = EgresoStockRepository;
            this.ArticuloRepository = ArticuloRepository;
        }

        #endregion


        #region IngresoStock

        public void AddIngresoStock(IngresoStock IngresoStock)
        {
            this.IngresoStockRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (IngresoStock.Cantidad < 0 || IngresoStock.Cantidad == null)
                {
                    IngresoStock.Cantidad = 0;
                }
                Articulo ToUpdate = this.ArticuloRepository.Get(IngresoStock.Articulo.Id);
                ToUpdate.Stock += IngresoStock.Cantidad;
                this.ArticuloRepository.Update(ToUpdate);
                IngresoStock.FechaCreacion = DateTime.Now;
                this.IngresoStockRepository.Add(IngresoStock);
            });
        }
        public IngresoStock GetIngresoStock(int Id)
        {
            IngresoStock IngresoStock = null;
            this.IngresoStockRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                IngresoStock = this.IngresoStockRepository.Get(Id);
            });
            return IngresoStock;
        }
        public IList<IngresoStock> GetAllIngresosStock(int IdArticulo, DateTime? start, DateTime? end)
        {
            IList<IngresoStock> IngresosStock = null;
            this.IngresoStockRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                IngresosStock = this.IngresoStockRepository.GetAllByArticulo(IdArticulo, _start, _end);
            });
            return IngresosStock;
        }
        //public SelectCombo GetAllIngresosStockByFilterCombo(SelectComboRequest req)
        //{
        //    SelectCombo SelectIngresoStockCombos = new SelectCombo();
        //    this.IngresoStockRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectIngresoStockCombos.Items = this.IngresoStockRepository.GetAllByFilter(req)
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                          text = x.Descripcion
        //                                                      }).ToList();
        //    });
        //    return SelectIngresoStockCombos;
        //}

        #endregion

        #region EgresoStock

        public void AddEgresoStock(EgresoStock EgresoStock)
        {
            this.EgresoStockRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (EgresoStock.Cantidad < 0 || EgresoStock.Cantidad == null)
                {
                    EgresoStock.Cantidad = 0;
                }
                Articulo ToUpdate = this.ArticuloRepository.Get(EgresoStock.Articulo.Id);
                if (ToUpdate.Stock-EgresoStock.Cantidad < 0)
                {
                    throw new BusinessException("La cantidad supera el Stock actual");
                }
                ToUpdate.Stock -= EgresoStock.Cantidad;
                this.ArticuloRepository.Update(ToUpdate);
                EgresoStock.FechaCreacion = DateTime.Now;
                this.EgresoStockRepository.Add(EgresoStock);
            });
        }
        public EgresoStock GetEgresoStock(int Id)
        {
            EgresoStock EgresoStock = null;
            this.EgresoStockRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                EgresoStock = this.EgresoStockRepository.Get(Id);
            });
            return EgresoStock;
        }
        public IList<EgresoStock> GetAllEgresosStock(int IdArticulo, DateTime? start, DateTime? end)
        {
            IList<EgresoStock> EgresosStock = null;
            this.EgresoStockRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                EgresosStock = this.EgresoStockRepository.GetAllByArticulo(IdArticulo, _start, _end);
            });
            return EgresosStock;
        }
        //public SelectCombo GetAllEgresosStockByFilterCombo(SelectComboRequest req)
        //{
        //    SelectCombo SelectEgresoStockCombos = new SelectCombo();
        //    this.EgresoStockRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectEgresoStockCombos.Items = this.EgresoStockRepository.GetAllByFilter(req)
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                          text = x.Descripcion
        //                                                      }).ToList();
        //    });
        //    return SelectEgresoStockCombos;
        //}


        #endregion
        
    }
}
