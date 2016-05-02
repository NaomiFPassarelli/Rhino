using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Repositories.Contabilidad;

namespace Woopin.SGC.Services
{
    public class ContabilidadReportService : IContabilidadReportService
    {
        #region Variables y Constructor
        private readonly IAsientoItemRepository AsientoItemRepository;

        public ContabilidadReportService(IAsientoItemRepository AsientoItemRepository)
        {
            this.AsientoItemRepository = AsientoItemRepository;
        }
        #endregion

        public IList<LibroIVA> GetAllLibroIVACompras(DateTime start, DateTime end)
        {
            IList<LibroIVA> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetLibroIVACompras(start, end);
            });
            return ret;
        }

        public IList<LibroIVA> GetAllLibroIVAVentas(DateTime start, DateTime end)
        {
            IList<LibroIVA> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetLibroIVAVentas(start, end);
            });
            return ret;
        }

        public IList<MayorItem> GetAllMayorProveedores(DateTime start, DateTime end)
        {
            IList<MayorItem> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetAllMayorProveedores(start, end);
            });
            return ret;
        }

        public IList<SumaSaldo> GetAllSumasYSaldos( DateTime start, DateTime end)
        {
            IList<SumaSaldo> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetAllSumasYSaldos(start, end);
            });
            return ret;
        }

        public IList<SumaSaldo> GetAllSumasYSaldosTree(DateTime start, DateTime end)
        {
            IList<SumaSaldo> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetAllSumasYSaldosTree(start, end);
            });
            return ret;
        }

        public IList<SumaSaldo> GetBalance(DateTime start, DateTime end)
        {
            IList<SumaSaldo> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetBalance(start, end);
            });
            return ret;
        }

        public IList<MayorItem> GetAllMayorClientes(DateTime start, DateTime end)
        {
            IList<MayorItem> ret = null;
            this.AsientoItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ret = this.AsientoItemRepository.GetAllMayorClientes(start, end);
            });
            return ret;
        }
    }
}
