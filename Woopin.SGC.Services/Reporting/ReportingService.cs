using PostSharp.Patterns.Diagnostics;
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
using Woopin.SGC.Model.Reporting;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Reporting;
using Woopin.SGC.Repositories.Ventas;
using PostSharp.Extensibility;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.CommonApp.Security;

namespace Woopin.SGC.Services
{
    public class ReportingService : IReportingService
    {
        #region VariablesyConstructor
        private readonly IVentasService VentasService;
        private readonly IGrupoEgresoRepository GrupoEgresoRepository;
        private readonly IGrupoIngresoRepository GrupoIngresoRepository;

        public ReportingService(IVentasService VentasService, 
            IGrupoEgresoRepository GrupoEgresoRepository, IGrupoIngresoRepository GrupoIngresoRepository)
        {
            this.VentasService = VentasService;
            this.GrupoEgresoRepository = GrupoEgresoRepository;
            this.GrupoIngresoRepository = GrupoIngresoRepository;
        }
        #endregion

        #region Grupo Ingreso
        
        public GrupoIngreso GetGrupoIngreso(int Id)
        {
            GrupoIngreso GrupoIngreso = null;
            this.GrupoIngresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                GrupoIngreso = this.GrupoIngresoRepository.Get(Id);
            });
            return GrupoIngreso;
        }

        public void AddGrupoIngreso(GrupoIngreso GrupoIngreso)
        {
            this.GrupoIngresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //level raiz
                if (GrupoIngreso.NodoPadre != null && GrupoIngreso.NodoPadre != null && GrupoIngreso.NodoPadre.Id != 0)
                {
                    GrupoIngreso gipadre = this.GrupoIngresoRepository.Get(GrupoIngreso.NodoPadre.Id);
                    GrupoIngreso.Level = gipadre.Level + 0;
                    GrupoIngreso.Raiz = gipadre.Raiz;
                }
                else
                {
                    GrupoIngreso.Level = 1;
                    GrupoIngreso.Raiz = this.GrupoIngresoRepository.GetLastRaiz() + 0;
                    GrupoIngreso.NodoPadre = null;
                }
                if (GrupoIngreso.Articulo == null || GrupoIngreso.Articulo.Id == 0 || GrupoIngreso.Articulo == null)
                    GrupoIngreso.Articulo = null;
                GrupoIngreso.Organizacion = Security.GetOrganizacion();
                this.GrupoIngresoRepository.Add(GrupoIngreso);
            });
        }

        public void UpdateGrupoIngreso(GrupoIngreso GrupoIngreso)
        {
            this.GrupoIngresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                GrupoIngreso oldGrupoIngreso = this.GrupoIngresoRepository.Get(GrupoIngreso.Id);
                oldGrupoIngreso.Descripcion = GrupoIngreso.Descripcion;

                //level raiz
                if (GrupoIngreso.NodoPadre != null && GrupoIngreso.NodoPadre != null && GrupoIngreso.NodoPadre.Id != 0)
                {
                    GrupoIngreso gipadre = this.GrupoIngresoRepository.Get(GrupoIngreso.NodoPadre.Id);
                    oldGrupoIngreso.Level = gipadre.Level + 1;
                    oldGrupoIngreso.Raiz = gipadre.Raiz;
                    oldGrupoIngreso.NodoPadre = GrupoIngreso.NodoPadre;
                }
                else
                {
                    oldGrupoIngreso.Level = 1;
                    oldGrupoIngreso.Raiz = this.GrupoIngresoRepository.GetLastRaiz() + 1;
                    oldGrupoIngreso.NodoPadre = null;
                }
                if (GrupoIngreso.Articulo == null || GrupoIngreso.Articulo.Id == 0)
                {
                    oldGrupoIngreso.Articulo = null;
                }
                else
                {
                    oldGrupoIngreso.Articulo = GrupoIngreso.Articulo;
                }
                this.GrupoIngresoRepository.Update(oldGrupoIngreso);
            });
        }

        public IList<GrupoIngreso> GetAllGruposIngresosTree(DateTime start, DateTime end)
        {
            IList<GrupoIngreso> GruposIngresos = null;
            this.GrupoIngresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                GruposIngresos = this.GrupoIngresoRepository.GetAllTree(start, end);
            });
            return GruposIngresos;
        }

        public SelectCombo GetAllGruposIngresosNoHoja(SelectComboRequest req)
        {
            SelectCombo SelectCombos = new SelectCombo();
            this.GrupoIngresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectCombos.Items = this.GrupoIngresoRepository.GetAllGruposIngresosNoHoja(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectCombos;
        }

        public void DeleteGrupoIngreso(int Id)
        {
            this.GrupoIngresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                GrupoIngreso GrupoIngreso = this.GrupoIngresoRepository.Get(Id);
                //hoja
                if (GrupoIngreso.Articulo == null || GrupoIngreso.Articulo.Id == 0)
                {
                    //buscar todos los hijos y eliminarlos
                    IList<GrupoIngreso> gil = this.GrupoIngresoRepository.GetAllHijos(Id);
                    foreach (GrupoIngreso gi in gil)
                    {
                        this.GrupoIngresoRepository.Delete(gi);
                    }
                }
                this.GrupoIngresoRepository.Delete(GrupoIngreso);
            });
        }

        #endregion

        #region Grupo Egreso
        public GrupoEgreso GetGrupoEgreso(int Id)
        {
            GrupoEgreso GrupoEgreso = null;
            this.GrupoEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                GrupoEgreso = this.GrupoEgresoRepository.Get(Id);
            });
            return GrupoEgreso;
        }

        public void AddGrupoEgreso(GrupoEgreso GrupoEgreso)
        {
            this.GrupoEgresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //level raiz
                if (GrupoEgreso.NodoPadre != null && GrupoEgreso.NodoPadre != null && GrupoEgreso.NodoPadre.Id != 0)
                {
                    GrupoEgreso gepadre = this.GrupoEgresoRepository.Get(GrupoEgreso.NodoPadre.Id);
                    GrupoEgreso.Level = gepadre.Level + 1;
                    GrupoEgreso.Raiz = gepadre.Raiz;
                }
                else
                {
                    GrupoEgreso.Level = 1;
                    GrupoEgreso.Raiz = this.GrupoEgresoRepository.GetLastRaiz() + 1;
                    GrupoEgreso.NodoPadre = null;
                }
                if (GrupoEgreso.Rubro == null || GrupoEgreso.Rubro.Id == 0 || GrupoEgreso.Rubro == null)
                    GrupoEgreso.Rubro = null;
                GrupoEgreso.Organizacion = Security.GetOrganizacion();
                this.GrupoEgresoRepository.Add(GrupoEgreso);
            });
        }

        public void UpdateGrupoEgreso(GrupoEgreso GrupoEgreso)
        {
            this.GrupoEgresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                GrupoEgreso oldGrupoEgreso = this.GrupoEgresoRepository.Get(GrupoEgreso.Id);
                oldGrupoEgreso.Descripcion = GrupoEgreso.Descripcion;
                
                //level raiz
                if (GrupoEgreso.NodoPadre != null && GrupoEgreso.NodoPadre != null && GrupoEgreso.NodoPadre.Id != 0)
                {
                    GrupoEgreso gepadre = this.GrupoEgresoRepository.Get(GrupoEgreso.NodoPadre.Id);
                    oldGrupoEgreso.Level = gepadre.Level + 1;
                    oldGrupoEgreso.Raiz = gepadre.Raiz;
                    oldGrupoEgreso.NodoPadre = GrupoEgreso.NodoPadre;
                }
                else
                {
                    oldGrupoEgreso.Level = 1;
                    oldGrupoEgreso.Raiz = this.GrupoEgresoRepository.GetLastRaiz() + 1;
                    oldGrupoEgreso.NodoPadre = null;
                }
                if (GrupoEgreso.Rubro == null || GrupoEgreso.Rubro.Id == 0)
                {
                    oldGrupoEgreso.Rubro = null;
                }
                else {
                    oldGrupoEgreso.Rubro = GrupoEgreso.Rubro;
                }
                this.GrupoEgresoRepository.Update(oldGrupoEgreso);
            });
        }

        public IList<GrupoEgreso> GetAllGruposEgresosTree(DateTime start, DateTime end)
        {
            IList<GrupoEgreso> GruposEgresos = null;
            this.GrupoEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                GruposEgresos = this.GrupoEgresoRepository.GetAllTree(start, end);
            });
            return GruposEgresos;
        }

         public SelectCombo GetAllGruposEgresosNoHoja(SelectComboRequest req)
         {
             SelectCombo SelectCombos = new SelectCombo();
             this.GrupoEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
             {
                 SelectCombos.Items = this.GrupoEgresoRepository.GetAllGruposEgresosNoHoja(req)
                                                               .Select(x => new SelectComboItem()
                                                               {
                                                                   id = x.Id,
                                                                   text = x.Descripcion
                                                               }).ToList();
             });
             return SelectCombos;
         }

         public void DeleteGrupoEgreso(int Id)
         {
             this.GrupoEgresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
             {
                 GrupoEgreso GrupoEgreso = this.GrupoEgresoRepository.Get(Id);
                 //hoja
                 if (GrupoEgreso.Rubro == null || GrupoEgreso.Rubro.Id == 0)
                 {
                     //buscar todos los hijos y eliminarlos
                     IList<GrupoEgreso> gel = this.GrupoEgresoRepository.GetAllHijos(Id);
                     foreach (GrupoEgreso ge in gel) {
                         this.GrupoEgresoRepository.Delete(ge);
                     }
                 }
                 this.GrupoEgresoRepository.Delete(GrupoEgreso);
             });
         }
        #endregion

    }
}
