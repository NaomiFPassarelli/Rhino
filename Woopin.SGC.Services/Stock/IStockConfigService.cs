using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Services
{
    public interface IStockConfigService
    {
        #region Articulo
        void AddArticulo(Articulo Articulo);
        void AddArticuloNT(Articulo Articulo);
        Articulo GetArticulo(int Id);
        Articulo GetArticuloConStock(int Id);
        void UpdateArticulo(Articulo Articulo);
        IList<Articulo> GetAllArticulos();
        void DeleteArticulos(List<int> Ids);
        SelectCombo GetAllArticulosByFilterCombo(SelectComboRequest req);
        SelectCombo GetAllArticulosConStockByFilterCombo(SelectComboRequest req);

        
        SelectCombo GetArticuloCombos();
        #endregion

        #region Rubro de Articulo
        void AddRubroArticulo(RubroArticulo RubroArticulo);
        void AddRubroArticuloNT(RubroArticulo RubroArticulo);
        RubroArticulo GetRubroArticulo(int Id);
        void UpdateRubroArticulo(RubroArticulo RubroArticulo);
        IList<RubroArticulo> GetAllRubroArticulos();
        void DeleteRubroArticulos(List<int> Ids);
        SelectCombo GetAllRubroArticulosByFilterCombo(SelectComboRequest req);
        SelectCombo GetRubroArticuloCombos();
        #endregion
        
    }
}
