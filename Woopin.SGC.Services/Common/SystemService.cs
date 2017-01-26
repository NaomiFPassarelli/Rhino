using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Common.App.Session;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Common;

namespace Woopin.SGC.Services
{
    public class SystemService : ISystemService
    {
        #region VariablesyConstructor

        private readonly IUsuarioRepository UsuarioRepository;
        private readonly IComboRepository ComboRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly IGeneralRepository GeneralRepository;
        private readonly ILogRepository LogRepository;
        private readonly IOrganizacionRepository OrganizacionRepository;
        private readonly IOrganizacionModuloRepository OrganizacionModuloRepository;
        private readonly IUsuarioOrganizacionRepository UsuarioOrganizacionRepository;
        private readonly IContabilidadConfigService ContabilidadConfigService;
        private readonly ITesoreriaConfigService TesoreriaConfigService;

        public SystemService(IUsuarioRepository UsuarioRepository, IComboRepository ComboRepository, IComboItemRepository ComboItemRepository, IGeneralRepository GeneralRepository, ILogRepository LogRepository, IOrganizacionRepository OrganizacionRepository,
                                            IUsuarioOrganizacionRepository UsuarioOrganizacionRepository, IContabilidadConfigService ContabilidadConfigService, ITesoreriaConfigService TesoreriaConfigService, IOrganizacionModuloRepository OrganizacionModuloRepository)
        {
            this.UsuarioRepository = UsuarioRepository;
            this.ComboRepository = ComboRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.GeneralRepository = GeneralRepository;
            this.LogRepository = LogRepository;
            this.OrganizacionRepository = OrganizacionRepository;
            this.OrganizacionModuloRepository = OrganizacionModuloRepository;
            this.UsuarioOrganizacionRepository = UsuarioOrganizacionRepository;
            this.ContabilidadConfigService = ContabilidadConfigService;
            this.TesoreriaConfigService = TesoreriaConfigService;
        }

        #endregion

        #region App

        
        public void InitializeSessionData(Usuario user)
        {
            SessionData sessionData = null;
            this.GeneralRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                sessionData = this.GeneralRepository.CreateSessionData(user);
            });
            HttpContext.Current.Session[SessionDataFactory.SESSION_KEY] = sessionData.SessionId;

            // Initialize logging parameters
            log4net.GlobalContext.Properties["Usuario_id"] = sessionData.CurrentUser.Id;
            log4net.GlobalContext.Properties["Organizacion_id"] = sessionData.CurrentOrganizacion.Id;

        }

        public void InitializeSessionData(JobHeader header)
        {
            SessionData sessionData = null;
            this.GeneralRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                sessionData = this.GeneralRepository.CreateSessionData(header);
            });
            // TODO: Debe guardarlo en el contexto del thread.
            JobSession.SessionId = sessionData.SessionId;

            // Initialize logging parameters
            log4net.ThreadContext.Properties["Usuario_id"] = sessionData.CurrentUser.Id;
            log4net.ThreadContext.Properties["Organizacion_id"] = sessionData.CurrentOrganizacion.Id;

        }
        public IList<Log> GetAllLogsByDates(int IdUsuario, int IdOrganizacion, DateTime start, DateTime end)
        {
            IList<Log> Logs = null;
            this.LogRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Logs = this.LogRepository.GetAllByDates(IdUsuario,IdOrganizacion, start, end);
            });
            return Logs;
        }
        #endregion

        #region Usuario
        public Usuario GetUsuario(int Id)
        {
            Usuario usuario = null;
            this.UsuarioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                usuario = this.UsuarioRepository.Get(Id);
            });
            return usuario;
        }
        public SelectCombo GetAllUsuariosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectUSuarioCombos = new SelectCombo();
            this.UsuarioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectUSuarioCombos.Items = this.UsuarioRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.NombreCompleto
                                                              }).ToList();
            });
            return SelectUSuarioCombos;
        }
        public Usuario GetUsuarioByUsername(string Username)
        {
            Usuario usuario = null;
            this.UsuarioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                usuario = this.UsuarioRepository.GetByUsername(Username);
            });
            return usuario;
        }
        public IList<Usuario> GetAllUsuarios()
        {
            IList<Usuario> Usuarios = null;
            this.UsuarioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Usuarios = this.UsuarioRepository.GetAll();
            });
            return Usuarios;
        }

        /// <summary>
        /// Creacion de usuarios dentro de la organizacion del usuario que esta creandolo.
        /// Le setea la organizacion actual, en la que lo estan creando.
        /// </summary>
        /// <param name="usuario">Usuario completo</param>
        [Loggable]
        public void AddUsuario(Usuario usuario)
        {
            this.UsuarioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                usuario.LastLogin = DateTime.Now;
                usuario.OrganizacionActual = new Organizacion() { Id = Security.GetOrganizacion().Id };
                this.UsuarioRepository.Add(usuario);
                UsuarioOrganizacion uo = new UsuarioOrganizacion()
                {
                    Organizacion = new Organizacion() { Id = Security.GetOrganizacion().Id },
                    Usuario = usuario
                };
                this.UsuarioOrganizacionRepository.Add(uo);
            });
        }

        public void UpdateUsuario(Usuario usuario)
        {
            this.UsuarioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.UsuarioRepository.Update(usuario);
            });
        }

        [Loggable]
        public void DeleteUsuarios(List<int> Ids)
        {
            this.UsuarioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Usuario usuario = this.UsuarioRepository.Get(Id);
                    usuario.Activo = false;
                    this.UsuarioRepository.Update(usuario);
                }
            });
        }

        [Loggable]
        public void CambiarActivo(List<int> Ids, bool Activo)
        {
            this.UsuarioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach(var Id in Ids)
                { 
                    Usuario usuario = this.UsuarioRepository.Get(Id);
                    usuario.Activo = Activo;
                    this.UsuarioRepository.Update(usuario);
                }
            });
        }

        /// <summary>
        /// Consulta para traer todos los usuarios para la administracion de organizaciones.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <param name="IdUsuario">Id del usuario a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los usuarios que cumplan con los criterios, menos el que esta loggeado.</returns>
        public IList<Usuario> GetAllUsuariosByOrganizacion(int IdOrganizacion)
        {
            IList<Usuario> Usuarios = null;
            this.UsuarioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Usuarios = this.UsuarioRepository.GetAllByOrganizacion(IdOrganizacion, SessionDataManager.Get().CurrentUser.Id);
            });
            return Usuarios;
        }

        /// <summary>
        /// Busqueda de usuarios conocidos por compartir alguna organizacion con el usuario loggeado.
        /// </summary>
        /// <returns>Listado de usuarios que conoce el usuario loggeado.</returns>
        public IList<Usuario> GetAllUsuariosMisOrganizaciones(int IdOrganizacion)
        {
            IList<Usuario> Usuarios = null;
            this.UsuarioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Usuarios = this.UsuarioRepository.GetAlMisOrganizaciones(SessionDataManager.Get().CurrentUser.Id, IdOrganizacion);
            });
            return Usuarios;
        }


        /// <summary>
        /// Elimina la relacion usuario-organizacion, para los usuarios seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de los usuarios a remover</param>
        /// <param name="IdOrganizacion">Id de la organización a limpiar</param>
        [Loggable]
        public void RemoverUsuariosOrganizacion(List<int> Ids, int IdOrganizacion)
        {
            this.UsuarioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    UsuarioOrganizacion usuarioOrg = this.UsuarioOrganizacionRepository.GetByIDs(Id, IdOrganizacion);
                    if(usuarioOrg != null)
                        this.UsuarioOrganizacionRepository.Delete(usuarioOrg);
                }
            });
        }

        /// <summary>
        /// Agregar las relaciones usuario-organizacion, para los usuarios seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de usuarios a agregar</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        [Loggable]
        public void AgregarUsuariosOrganizacion(List<int> Ids, int IdOrganizacion)
        {
            this.UsuarioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    UsuarioOrganizacion usuarioOrg = this.UsuarioOrganizacionRepository.GetByIDs(Id, IdOrganizacion);
                    if (usuarioOrg == null)
                    {
                        UsuarioOrganizacion uo = new UsuarioOrganizacion()
                        {
                            Organizacion = new Organizacion() { Id = IdOrganizacion },
                            Usuario = new Usuario() { Id = Id }
                        };
                        this.UsuarioOrganizacionRepository.Add(uo);
                    }
                        
                }
            });
        }

        #endregion

        #region General
        public Dashboard GetDashboard()
        {
            Dashboard dashboard = null;
            this.GeneralRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                dashboard = this.GeneralRepository.GetDashboard();
            });
            return dashboard;
        }
        #endregion

        #region Organizacion
        public Organizacion GetOrganizacion(int Id)
        {
            Organizacion Organizacion = null;
            this.OrganizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Organizacion = this.OrganizacionRepository.Get(Id);
            });
            return Organizacion;
        }

        public IList<Organizacion> GetAllOrganizaciones()
        {
            IList<Organizacion> Organizaciones = null;
            this.OrganizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Organizaciones = this.OrganizacionRepository.GetAll();
            });
            return Organizaciones;
        }

        [Loggable]
        public void AddOrganizacion(Organizacion Organizacion)
        {
            this.OrganizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Organizacion.Administrador = SessionDataManager.Get().CurrentUser;
                Organizacion.Activo = true;
                this.OrganizacionRepository.Add(Organizacion);
                
                
                
                // Usuarios en organizacion
                UsuarioOrganizacion uo = new UsuarioOrganizacion() {
                    Organizacion = Organizacion,
                    Usuario = Organizacion.Administrador
                };
                this.UsuarioOrganizacionRepository.Add(uo);

                Organizacion OrganizacionOriginaria = Security.GetOrganizacion();
                //Para crear tanto las cuentas como los valores para la organizacion que se esta creando
                Security.SetOrganizacion(Organizacion);

                // Plan de Cuentas Generico.
                List<Cuenta> cuentas = InstalacionHelper.GetCuentasContables();
                foreach (var cuenta in cuentas)
                {
                    this.ContabilidadConfigService.AddCuentaNT(cuenta);
                }

                // Valores Genericos
                List<Woopin.SGC.Model.Tesoreria.Valor> valores = InstalacionHelper.GetValores();
                foreach (var valor in valores)
                {
                    this.TesoreriaConfigService.AddValorNT(valor);
                }
                //Vuelve a la organizacion en la que esta realmente logueado el usuario
                Security.SetOrganizacion(OrganizacionOriginaria);
            });
        }

        /// <summary>
        /// Actualiza la organización enviada.
        /// </summary>
        /// <param name="Organizacion">Objeto organizacion a actualizar</param>
        /// 
        [Loggable]
        public void UpdateOrganizacion(Organizacion Organizacion)
        {
            this.OrganizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Organizacion orgToUpdate = this.OrganizacionRepository.Get(Organizacion.Id);
                orgToUpdate.Actividad = Organizacion.Actividad;
                orgToUpdate.Categoria = Organizacion.Categoria;
                orgToUpdate.CodigoPostal = Organizacion.CodigoPostal;
                orgToUpdate.CUIT = Organizacion.CUIT;
                orgToUpdate.Domicilio = Organizacion.Domicilio;
                orgToUpdate.Email = Organizacion.Email;
                orgToUpdate.IngresosBrutos = Organizacion.IngresosBrutos;
                orgToUpdate.NombreFantasia = Organizacion.NombreFantasia;
                orgToUpdate.Provincia = Organizacion.Provincia;
                orgToUpdate.RazonSocial = Organizacion.RazonSocial;
                orgToUpdate.Telefono = Organizacion.Telefono;
                orgToUpdate.ImagePath = Organizacion.ImagePath;
                this.OrganizacionRepository.Update(orgToUpdate);

                //Actualizo la organizacion del que esta loggeado, si esta con esa organizacion.
                if (orgToUpdate.Id == Security.GetOrganizacion().Id)
                {
                    SessionDataManager.SetOrganizacion(orgToUpdate);
                }
                    
            });
        }
        public void DeleteOrganizaciones(List<int> Ids)
        {
            this.OrganizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Organizacion Organizacion = this.OrganizacionRepository.Get(Id);
                    Organizacion.Activo = false;
                    this.OrganizacionRepository.Update(Organizacion);
                }
            });
        }
        public Organizacion GetCurrentOrganizacion()
        {
            Organizacion Organizacion = null;
            this.OrganizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Organizacion = this.UsuarioRepository.GetCompleto(Security.GetCurrentUser().Id).OrganizacionActual;
            });
            return Organizacion;
        }

        /// <summary>
        /// Busqueda de las organizaciones en las que participa  el usuario loggeado.
        /// </summary>
        /// <returns>Listado de organizaciones a las cual pertenece el usuario loggeado.</returns>
        public IList<Organizacion> GetMisOrganizaciones()
        {
            IList<Organizacion> Organizaciones = null;
            this.OrganizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Organizaciones = this.OrganizacionRepository.GetAllMine();
            });
            return Organizaciones;
        }

        /// <summary>
        /// Setea la organizacion actual del usuario loggeado.
        /// </summary>
        /// <param name="Id">ID de la organizacion que desea operar</param>
        /// 
        [Loggable]
        public void SetCurrentOrganizacion(int Id)
        {
            this.OrganizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                // Busqueda de los nuevos datos desde base.
                Usuario user = this.UsuarioRepository.Get(Security.GetCurrentUser().Id);
                Organizacion org = this.OrganizacionRepository.Get(Id);

                // Cambio en base de la organizacion
                user.OrganizacionActual = org;
                this.UsuarioRepository.Update(user);

                // Busqueda de los nuevos datos desde base.
                IList<ModulosSistemaGestion> modulos = this.GetAllModulosByOrganizacion(org.Id);

                // Actualizo la información de la sesion.
                Security.SetUsuario(user);
                Security.SetOrganizacion(org);
                Security.SetModulos(modulos);
            });
        }

        #endregion

        #region

        /// <summary>
        /// Consulta para traer todos los modulos para la organizacion actual.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los modulos que cumplan con los criterios.</returns>
        public IList<ModulosSistemaGestion> GetAllModulosByOrganizacion(int IdOrganizacion)
        {
            IList<ModulosSistemaGestion> Modulos = null;
            this.OrganizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                //Modulos = this.OrganizacionRepository.GetAllModulosByOrganizacion(IdOrganizacion);
                Modulos = this.OrganizacionModuloRepository.GetAllModulosByOrganizacion(IdOrganizacion);
            });
            return Modulos;
        }


        /// <summary>
        /// Elimina la relacion modulo-organizacion, para los modulos seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de los modulos a remover</param>
        /// <param name="IdOrganizacion">Id de la organización a limpiar</param>
        [Loggable]
        public void RemoverModulosOrganizacion(List<int> Ids, int IdOrganizacion)
        {
            this.OrganizacionModuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    OrganizacionModulo moduloOrg = this.OrganizacionModuloRepository.GetByIDs(Id, IdOrganizacion);
                    if (moduloOrg != null)
                        this.OrganizacionModuloRepository.Delete(moduloOrg);
                }
            });
        }

        /// <summary>
        /// Agregar las relaciones modulo-organizacion, para los modulos seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de modulos a agregar</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        [Loggable]
        public void AgregarModulosOrganizacion(List<int> Ids, int IdOrganizacion)
        {
            this.OrganizacionModuloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    OrganizacionModulo moduloOrg = this.OrganizacionModuloRepository.GetByIDs(Id, IdOrganizacion);
                    if (moduloOrg == null)
                    {
                        OrganizacionModulo uo = new OrganizacionModulo()
                        {
                            Organizacion = new Organizacion() { Id = IdOrganizacion },
                            ModulosSistemaGestion = (ModulosSistemaGestion)Id
                        };
                        this.OrganizacionModuloRepository.Add(uo);
                    }

                }
            });
        }

        
        #endregion

        


    }
}
