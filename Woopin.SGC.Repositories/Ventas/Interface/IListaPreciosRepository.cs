using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public interface IListaPreciosRepository : IRepository<ListaPreciosItem>
    {
        IList<ListaPreciosItem> GetAllByCliente(int IdCliente);
        IList<ListaPreciosItem> GetAllByGrupo(int IdGrupo);
        IList<ListaPreciosItem> GetAllDefault();
        void SaveByCliente(ListaPreciosItem ListaPreciosItem, int Id);
        void SaveByGrupo(ListaPreciosItem ListaPreciosItem, int Id);
        void SaveByDefault(ListaPreciosItem ListaPreciosItem);
        ListaPreciosItem GetForVentas(int IdModalidad, int IdCliente);
    }
}
