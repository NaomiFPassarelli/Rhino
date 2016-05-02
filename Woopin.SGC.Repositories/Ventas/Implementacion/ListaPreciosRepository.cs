using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Compras;
using NHibernate.Transform;
using NHibernate.Criterion;
using NHibernate;
using NHibernate.Dialect.Function;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Helpers;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Repositories.Ventas
{
    public class ListaPreciosRepository : BaseSecuredRepository<ListaPreciosItem>, IListaPreciosRepository
    {
        public ListaPreciosRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public ListaPreciosItem GetForVentas(int IdArticulo, int IdCliente)
        {
            ListaPreciosItem item = null;

            // Busco si tiene un precio el cliente.
            item = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Cliente.Id == IdCliente && x.Articulo.Id == IdArticulo)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
            if (item != null) return item;

            // Busco si tiene un precio el Grupo.
            GrupoEconomico grupo = this.GetSessionFactory().GetSession().QueryOver<Cliente>().Where(x => x.Id == IdCliente).GetFilterBySecurity().SingleOrDefault().Master;
            if(grupo != null)
            {
                item = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Grupo.Id == grupo.Id && x.Articulo.Id == IdArticulo)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
                if (item != null) return item;
            }
            // Caso contrario, devuelvo el default.
            return this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Grupo == null && x.Cliente == null && x.Articulo.Id == IdArticulo)
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
        }
        public IList<ListaPreciosItem> GetAllByCliente(int IdCliente)
        {
            GrupoEconomico grupo = this.GetSessionFactory().GetSession().QueryOver<Cliente>().Where(x => x.Id == IdCliente).GetFilterBySecurity().SingleOrDefault().Master;

            // Busco todo el listado asociado del cliente
            IList<ListaPreciosItem> itemsCliente = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Cliente.Id == IdCliente)
                                                        .Fetch(x => x.Articulo).Eager
                                                        .GetFilterBySecurity()
                                                        .List();
            // Busco todo el listado asociado al grupo del cliente si tiene grupo.
            IList<ListaPreciosItem> itemsGrupo = null;
            if (grupo != null)
            {
                itemsGrupo = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Grupo.Id == grupo.Id)
                                                        .Fetch(x => x.Articulo).Eager
                                                        .GetFilterBySecurity()
                                                        .List();
            }

            IList<ListaPreciosItem> itemsDefault = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Cliente == null && x.Grupo == null)
                                                        .Fetch(x => x.Articulo).Eager
                                                        .GetFilterBySecurity()
                                                        .List();

            List<ListaPreciosItem> items = new List<ListaPreciosItem>();

            foreach(var itemDefault in itemsDefault)
            {
                ListaPreciosItem itemCliente = itemsCliente.Where(x => x.Articulo.Id == itemDefault.Articulo.Id).SingleOrDefault();
                ListaPreciosItem itemGrupo = null;
                if(grupo != null)
                {
                    itemGrupo = itemsGrupo.Where(x => x.Articulo.Id == itemDefault.Articulo.Id).SingleOrDefault();
                }
                if(itemCliente != null)
                {
                    items.Add(itemCliente);
                }
                else if (grupo != null && itemGrupo != null)
                {
                    items.Add(itemGrupo);
                }
                else
                {
                    items.Add(itemDefault);
                }
            }

            return items.OrderBy(x => x.Articulo.Descripcion).ToList();
            
        }
        public IList<ListaPreciosItem> GetAllByGrupo(int IdGrupo)
        {
            // Busco todo el listado asociado al grupo del cliente si tiene grupo.
            IList<ListaPreciosItem> itemsGrupo =  itemsGrupo = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Grupo.Id == IdGrupo)
                                                        .Fetch(x => x.Articulo).Eager
                                                        .GetFilterBySecurity()
                                                        .List();

            IList<ListaPreciosItem> itemsDefault = this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Cliente == null && x.Grupo == null)
                                                        .Fetch(x => x.Articulo).Eager
                                                        .GetFilterBySecurity()
                                                        .List();

            List<ListaPreciosItem> items = new List<ListaPreciosItem>();

            foreach (var itemDefault in itemsDefault)
            {
                ListaPreciosItem itemGrupo = itemsGrupo.Where(x => x.Articulo.Id == itemDefault.Articulo.Id).SingleOrDefault();
                if (itemGrupo != null)
                {
                    items.Add(itemGrupo);
                }
                else
                {
                    items.Add(itemDefault);
                }
            }

            return items.OrderBy(x => x.Articulo.Descripcion).ToList();

        }
        public IList<ListaPreciosItem> GetAllDefault()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ListaPreciosItem>()
                                                        .Where(x => x.Cliente == null && x.Grupo == null)
                                                        .Fetch(x => x.Articulo).Eager
                                                        .GetFilterBySecurity()
                                                        .List()
                                                        .OrderBy(x => x.Articulo.Descripcion).ToList();
        }
        public void SaveByCliente(ListaPreciosItem ListaPreciosItem, int Id)
        {
            ListaPreciosItem savedItem = this.Get(ListaPreciosItem.Id);

            if (savedItem.Cliente != null && savedItem.Cliente.Id == Id)  // Esta editando uno de este cliente
            {
                savedItem.Precio = ListaPreciosItem.Precio;
                this.Update(savedItem);
            }
            else // No existe para el cliente, puede ser que venga del default o del grupo ec.
            {
                ListaPreciosItem newItem = new ListaPreciosItem();
                newItem.Precio = ListaPreciosItem.Precio;
                newItem.Cliente = new Model.Ventas.Cliente() { Id = Id };
                newItem.Articulo = savedItem.Articulo;
                newItem.Organizacion = savedItem.Organizacion;
                this.Add(newItem);
            }
        }
        public void SaveByGrupo(ListaPreciosItem ListaPreciosItem, int Id)
        {
            ListaPreciosItem savedItem = this.Get(ListaPreciosItem.Id);

            if (savedItem.Grupo != null && savedItem.Grupo.Id == Id)  // Esta editando uno de este grupo
            {
                savedItem.Precio = ListaPreciosItem.Precio;
                this.Update(savedItem);
            }
            else // No existe para el grupo, viene del defaul.
            {
                ListaPreciosItem newItem = new ListaPreciosItem();
                newItem.Precio = ListaPreciosItem.Precio;
                newItem.Grupo = new Model.Ventas.GrupoEconomico() { Id = Id };
                newItem.Articulo = savedItem.Articulo;
                newItem.Organizacion = savedItem.Organizacion;
                this.Add(newItem);
            }
        }
        public void SaveByDefault(ListaPreciosItem ListaPreciosItem)
        {
            ListaPreciosItem savedItem = this.Get(ListaPreciosItem.Id);
            savedItem.Precio = ListaPreciosItem.Precio;
            this.Update(savedItem);
        }
    }
}
