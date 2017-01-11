using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Cooperativa;
using Woopin.SGC.Services;
using Woopin.SGC.Services.Afip;
using Woopin.SGC.Services.Common;
using Woopin.SGC.Common.Helpers;
using WebMatrix.WebData;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Services.Afip.Model;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Common.App.Session;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Woopin.SGC.Common.Validations;
using System.ComponentModel.DataAnnotations;

namespace Woopin.SGC.Web.Scheduler
{
    public class CooperativaJobs
    {
        private readonly ISystemService SystemService;
        private readonly ICooperativaConfigService CooperativaConfigService;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CooperativaJobs(ICooperativaConfigService CooperativaConfigService,  ISystemService SystemService)
        {
            this.SystemService = SystemService;
            this.CooperativaConfigService = CooperativaConfigService;
        }

        public void ImportarAsociados(string path, JobHeader jobHeader)
        {
            // Initialize job sessiondata
            this.SystemService.InitializeSessionData(jobHeader);

            IWorkbook hssfwb;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(path).Contains("xlsx"))
                {
                    hssfwb = new XSSFWorkbook(file);
                }
                else
                {
                    hssfwb = new HSSFWorkbook(file);
                }
            }

            ISheet sheet = hssfwb.GetSheet("Asociados");
            List<Asociado> Asociados = new List<Asociado>();
            
            //for (int row = 1; row <= sheet.LastRowNum; row++)
            for (int row = 1; sheet.GetRow(row) != null; row++)
            {
                IRow currentRow = sheet.GetRow(row);

                ICell cuitCell = currentRow.GetCell(0);
                string Cuit = null;
                if (cuitCell != null)
                {
                    Cuit = (currentRow.GetCell(0).CellType == CellType.String) ? currentRow.GetCell(0).StringCellValue : currentRow.GetCell(0).NumericCellValue.ToString();
                    //Cuit = cuitCell.StringCellValue;
                }
                ICell dniCell = currentRow.GetCell(1);
                string DNI = null;
                if (dniCell != null)
                {
                    DNI = dniCell.StringCellValue;
                }
                ICell ciCell = currentRow.GetCell(2);
                string CI = null;
                if (ciCell != null)
                {
                    CI = ciCell.StringCellValue;
                }
                ICell leCell = currentRow.GetCell(3);
                string LE = null;
                if (leCell != null)
                {
                    LE = leCell.StringCellValue;
                }
                ICell lcCell = currentRow.GetCell(4);
                string LC = null;
                if (lcCell != null)
                {
                    LC = lcCell.StringCellValue;
                }

                if (currentRow != null &&
                    ((Cuit != null && Cuit.Length > 0) ||
                    (DNI != null && DNI.Length > 0) ||
                    (CI != null && CI.Length > 0) ||
                    (LE != null && LE.Length > 0) ||
                    (LC != null && LC.Length > 0)))
                {
                    // Creates the comment cell
                    ICell statusCell = currentRow.CreateCell(34);
                    sheet.AutoSizeColumn(34);
                    statusCell.CellStyle.WrapText = true;

                    try
                    {
                        Asociado c = new Asociado(){};
                        int[] obligatorios = new int[] {5,6,16,29,30};
                        int columnsExcel = 34;
                        for (int u = 1; u < columnsExcel; u++)
                        {
                            //if (currentRow.GetCell(u) != null)
                            if (currentRow.GetCell(u).CellType != CellType.Blank)
                            {

                                CellType type = currentRow.GetCell(u).CellType;
                                if (type == CellType.String)
                                {
                                    currentRow.GetCell(u).SetCellType(CellType.String);
                                }
                                else if (type == CellType.Numeric)
                                {
                                    currentRow.GetCell(u).SetCellType(CellType.Numeric);
                                }
                                //TODO ver CUIT, DNI, CI, LE, LC
                                //Campos NO obligatorios
                                switch (u) {
                                    case 7:
                                        c.Localizacion = new Localizacion() { Nombre = currentRow.GetCell(7).StringCellValue };
                                        break;
                                    case 8:
                                        c.Direccion = (currentRow.GetCell(8).CellType == CellType.String) ? currentRow.GetCell(8).StringCellValue : currentRow.GetCell(8).NumericCellValue.ToString();
                                        break;
                                    case 9:
                                        c.Numero = (currentRow.GetCell(9).CellType == CellType.String) ? currentRow.GetCell(9).StringCellValue : currentRow.GetCell(9).NumericCellValue.ToString();
                                        break;
                                    case 10:
                                        c.Departamento = (currentRow.GetCell(10).CellType == CellType.String) ? currentRow.GetCell(10).StringCellValue : currentRow.GetCell(10).NumericCellValue.ToString();
                                        break;
                                    case 11:
                                        c.Piso = (currentRow.GetCell(11).CellType == CellType.String) ? currentRow.GetCell(11).StringCellValue : currentRow.GetCell(11).NumericCellValue.ToString();
                                        break;
                                    case 12:
                                        c.CodigoPostal = (currentRow.GetCell(12).CellType == CellType.String) ? currentRow.GetCell(12).StringCellValue : currentRow.GetCell(12).NumericCellValue.ToString();
                                        break;
                                    case 13:
                                        c.Nacionalidad = new ComboItem() { Data = currentRow.GetCell(13).StringCellValue };
                                        break;
                                    case 14:
                                        c.EstadoCivil = new ComboItem() { Data = currentRow.GetCell(14).StringCellValue };
                                        break;
                                    case 15:
                                        c.Telefono = (currentRow.GetCell(15).CellType == CellType.String) ? currentRow.GetCell(15).StringCellValue : currentRow.GetCell(15).NumericCellValue.ToString();
                                        break;
                                    case 17:
                                        c.FechaNacimiento = Convert.ToDateTime(currentRow.GetCell(17).DateCellValue);
                                        break;
                                    case 18:
                                        c.LugarNacimiento = currentRow.GetCell(18).StringCellValue;
                                        break;
                                    case 19:
                                        c.RecomendadoPor = currentRow.GetCell(19).StringCellValue;
                                        break;
                                    case 20:
                                        c.NroCarnetConductor = (currentRow.GetCell(20).CellType == CellType.String) ? currentRow.GetCell(20).StringCellValue : currentRow.GetCell(20).NumericCellValue.ToString();
                                        break;
                                    case 21:
                                        c.CategoriaConductor = (currentRow.GetCell(21).CellType == CellType.String) ? currentRow.GetCell(21).StringCellValue : currentRow.GetCell(21).NumericCellValue.ToString();
                                        break;
                                    case 22:
                                        c.MarcaVehiculo = (currentRow.GetCell(22).CellType == CellType.String) ? currentRow.GetCell(22).StringCellValue : currentRow.GetCell(22).NumericCellValue.ToString();
                                        break;
                                    case 23:
                                        c.ModeloVehiculo = (currentRow.GetCell(23).CellType == CellType.String) ? currentRow.GetCell(23).StringCellValue : currentRow.GetCell(23).NumericCellValue.ToString();
                                        break;
                                    case 24:
                                        c.NroChapaVehiculo = (currentRow.GetCell(24).CellType == CellType.String) ? currentRow.GetCell(24).StringCellValue : currentRow.GetCell(24).NumericCellValue.ToString();
                                        break;
                                    case 25:
                                        c.FechaNotificacion = Convert.ToDateTime(currentRow.GetCell(25).DateCellValue);
                                        break;
                                    case 26:
                                        c.FechaEgreso = Convert.ToDateTime(currentRow.GetCell(26).DateCellValue);
                                        break;
                                    case 27:
                                        c.FechaActaIngreso = Convert.ToDateTime(currentRow.GetCell(27).DateCellValue);
                                        break;
                                    case 28:
                                        c.Cargo = (currentRow.GetCell(28).CellType == CellType.String) ? currentRow.GetCell(28).StringCellValue : currentRow.GetCell(28).NumericCellValue.ToString();
                                        break;
                                    case 31:
                                        c.Padre = (currentRow.GetCell(31).CellType == CellType.String) ? currentRow.GetCell(31).StringCellValue : currentRow.GetCell(31).NumericCellValue.ToString();
                                        break;
                                    case 32:
                                        c.Madre = (currentRow.GetCell(32).CellType == CellType.String) ? currentRow.GetCell(32).StringCellValue : currentRow.GetCell(32).NumericCellValue.ToString();
                                        break;
                                    case 33:
                                        c.NroPolicia = (currentRow.GetCell(33).CellType == CellType.String) ? currentRow.GetCell(33).StringCellValue : currentRow.GetCell(33).NumericCellValue.ToString();
                                        break;
                                }
                            }
                            else if (obligatorios.Contains(u)) //Campos obligatorios
                            {
                                string msg = "Todos los campos obligatorios deben estar completos";
                                statusCell.SetCellValue(msg);
                            }
                            else
                            { //Campos NO obligatorios
                                switch (u) {
                                    case 7:
                                        c.Localizacion = null;
                                        break;
                                    case 8:
                                        c.Direccion = null;
                                        break;
                                    case 9:
                                        c.Numero = null;
                                        break;
                                    case 10:
                                        c.Departamento = null;
                                        break;
                                    case 11:
                                        c.Piso = null;
                                        break;
                                    case 12:
                                        c.CodigoPostal = null;
                                        break;
                                    case 13:
                                        c.Nacionalidad = null;
                                        break;
                                    case 14:
                                        c.EstadoCivil = null;
                                        break;
                                    case 15:
                                        c.Telefono = null;
                                        break;
                                    case 17:
                                        c.FechaNacimiento = null;
                                        break;
                                    case 18:
                                        c.LugarNacimiento = null;
                                        break;
                                    case 19:
                                        c.RecomendadoPor = null;
                                        break;
                                    case 20:
                                        c.NroCarnetConductor = null;
                                        break;
                                    case 21:
                                        c.CategoriaConductor = null;
                                        break;
                                    case 22:
                                        c.MarcaVehiculo = null;
                                        break;
                                    case 23:
                                        c.ModeloVehiculo = null;
                                        break;
                                    case 24:
                                        c.NroChapaVehiculo = null;
                                        break;
                                    case 25:
                                        c.FechaNotificacion = null;
                                        break;
                                    case 26:
                                        c.FechaEgreso = null;
                                        break;
                                    case 27:
                                        c.FechaActaIngreso = null;
                                        break;
                                    case 28:
                                        c.Cargo = null;
                                        break;
                                    case 31:
                                        c.Padre = null;
                                        break;
                                    case 32:
                                        c.Madre = null;
                                        break;
                                    case 33:
                                        c.NroPolicia = null;
                                        break;
                                }
                            }
                        }

                        //Campos obligatorios
                        //c.CUIT = Cuit;
                        //c.DNI = (currentRow.GetCell(1).CellType == CellType.String) ? currentRow.GetCell(1).StringCellValue : currentRow.GetCell(1).NumericCellValue.ToString();
                        c.Nombre = currentRow.GetCell(5).StringCellValue;
                        c.Apellido = currentRow.GetCell(6).StringCellValue;
                        c.FechaIngreso = Convert.ToDateTime(currentRow.GetCell(16).DateCellValue);
                        c.CantidadAbonos = int.Parse((currentRow.GetCell(29).CellType == CellType.String) ? currentRow.GetCell(29).StringCellValue : currentRow.GetCell(29).NumericCellValue.ToString());
                        c.ImportePago = decimal.Parse((currentRow.GetCell(30).CellType == CellType.String) ? currentRow.GetCell(30).StringCellValue : currentRow.GetCell(30).NumericCellValue.ToString());

                        
                        // Model validations.
                        List<ValidationResult> validations;
                        bool isModelValid = new ModelValidatorHelper<Asociado>(c).Validate(out validations);
                        if (!isModelValid)
                        {
                            string message = String.Join("\n", validations.Select(x => x.ErrorMessage).ToArray());
                            throw new BusinessException(message);
                        }

                        this.CooperativaConfigService.ImportAsociado(c);
                        statusCell.SetCellValue("Importado");
                    }
                    catch (BusinessException be)
                    {
                        statusCell.SetCellValue(be.ErrorMessage);
                    }
                    catch (Exception e)
                    {
                        string msg = "Ocurrio un error inesperado al importar. Vuelva a intentarlo.";
                        statusCell.SetCellValue(msg);
                    }

                }
            }
            string currentFileName = Path.GetFileName(path);
            string newFileName = "resultado_asociados_" + currentFileName;
            string newPath = path.Replace(currentFileName,newFileName);
            using (FileStream file = new FileStream(newPath, FileMode.Create, FileAccess.Write))
            {
                hssfwb.Write(file);
                file.Close();
            }   
 
            // ADDNOTIFICATION
        }

    }
}