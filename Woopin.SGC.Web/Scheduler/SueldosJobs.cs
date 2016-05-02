using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Sueldos;
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
    public class SueldosJobs
    {
        private readonly ISystemService SystemService;
        private readonly ISueldosConfigService SueldosConfigService;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SueldosJobs(ISueldosConfigService SueldosConfigService,  ISystemService SystemService)
        {
            this.SystemService = SystemService;
            this.SueldosConfigService = SueldosConfigService;
        }

        public void ImportarEmpleados(string path, JobHeader jobHeader)
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

            ISheet sheet = hssfwb.GetSheet("Empleados");
            List<Empleado> Empleados = new List<Empleado>();
            
            //for (int row = 1; row <= sheet.LastRowNum; row++)
            for (int row = 1; sheet.GetRow(row) != null; row++)
            {
                IRow currentRow = sheet.GetRow(row);

                ICell cuitCell = currentRow.GetCell(0);
                string Cuit = null;
                if (cuitCell != null)
                {
                    Cuit = cuitCell.StringCellValue;
                }

                if (currentRow != null && Cuit != null && Cuit.Length > 0)
                {
                    // Creates the comment cell
                    ICell statusCell = currentRow.CreateCell(24);
                    sheet.AutoSizeColumn(24);
                    statusCell.CellStyle.WrapText = true;

                    try
                    {
                        //Empleado c = new Empleado()
                        //{
                        //    DNI = currentRow.GetCell(1).StringCellValue,
                        //    CUIT = Cuit,
                        //    Nombre = currentRow.GetCell(5).StringCellValue,
                        //    Apellido = currentRow.GetCell(6).StringCellValue,
                        //    Email = currentRow.GetCell(7).StringCellValue,
                        //    Localizacion = new Localizacion() { Nombre = currentRow.GetCell(8).StringCellValue },
                        //    Direccion = currentRow.GetCell(9).StringCellValue,
                        //    Numero = currentRow.GetCell(10).StringCellValue,
                        //    Departamento = currentRow.GetCell(11).StringCellValue,
                        //    Piso = currentRow.GetCell(12).StringCellValue,
                        //    CodigoPostal = currentRow.GetCell(13).StringCellValue,
                        //    Nacionalidad = new ComboItem() { Data = currentRow.GetCell(14).StringCellValue },
                        //    EstadoCivil = new ComboItem() { Data = currentRow.GetCell(15).StringCellValue },
                        //    Sexo = new ComboItem() { Data = currentRow.GetCell(16).StringCellValue },
                        //    Telefono = currentRow.GetCell(17).StringCellValue,
                        //    SueldoBrutoMensual = Convert.ToDecimal(currentRow.GetCell(18).StringCellValue),
                        //    SueldoBrutoHora = Convert.ToDecimal(currentRow.GetCell(19).StringCellValue),
                        //    FechaIngreso = Convert.ToDateTime(currentRow.GetCell(20).StringCellValue),
                        //    FechaNacimiento = Convert.ToDateTime(currentRow.GetCell(21).StringCellValue),
                        //    Categoria = new ComboItem() { Data = currentRow.GetCell(22).StringCellValue },
                        //    Tarea = new ComboItem() { Data = currentRow.GetCell(23).StringCellValue }
                        //};

                        Empleado c = new Empleado(){};

                        int[] obligatorios = new int[] {1,2,3,17};
                        for (int u = 1; u < 21; u++)
                        {
                            if (currentRow.GetCell(u) != null)
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

                                //Campos NO obligatorios
                                switch (u) {
                                    case 4:
                                        c.Email = (currentRow.GetCell(4).CellType == CellType.String) ? currentRow.GetCell(4).StringCellValue : currentRow.GetCell(4).NumericCellValue.ToString();
                                        break;
                                    case 5:
                                        c.Localizacion = new Localizacion() { Nombre = currentRow.GetCell(5).StringCellValue };
                                        break;
                                    case 6:
                                        c.Direccion = (currentRow.GetCell(6).CellType == CellType.String) ? currentRow.GetCell(6).StringCellValue : currentRow.GetCell(6).NumericCellValue.ToString();
                                        break;
                                    case 7:
                                        c.Numero = (currentRow.GetCell(7).CellType == CellType.String) ? currentRow.GetCell(7).StringCellValue : currentRow.GetCell(7).NumericCellValue.ToString();
                                        break;
                                    case 8:
                                        c.Departamento = (currentRow.GetCell(8).CellType == CellType.String) ? currentRow.GetCell(8).StringCellValue : currentRow.GetCell(8).NumericCellValue.ToString();
                                        break;
                                    case 9:
                                        c.Piso = (currentRow.GetCell(9).CellType == CellType.String) ? currentRow.GetCell(9).StringCellValue : currentRow.GetCell(9).NumericCellValue.ToString();
                                        break;
                                    case 10:
                                        c.CodigoPostal = (currentRow.GetCell(10).CellType == CellType.String) ? currentRow.GetCell(10).StringCellValue : currentRow.GetCell(10).NumericCellValue.ToString();
                                        break;
                                    case 11:
                                        c.Nacionalidad = new ComboItem() { Data = currentRow.GetCell(11).StringCellValue };
                                        break;
                                    case 12:
                                        c.EstadoCivil = new ComboItem() { Data = currentRow.GetCell(12).StringCellValue };
                                        break;
                                    case 13:
                                        c.Sexo = new ComboItem() { Data = currentRow.GetCell(13).StringCellValue };
                                        break;
                                    case 14:
                                        c.Telefono = (currentRow.GetCell(14).CellType == CellType.String) ? currentRow.GetCell(14).StringCellValue : currentRow.GetCell(14).NumericCellValue.ToString();
                                        break;
                                    case 15:
                                        c.SueldoBrutoMensual = Convert.ToDecimal(currentRow.GetCell(15).NumericCellValue);
                                        break;
                                    case 16:
                                        c.SueldoBrutoHora = Convert.ToDecimal(currentRow.GetCell(16).NumericCellValue);
                                        break;
                                    case 18:
                                        c.FechaNacimiento = Convert.ToDateTime(currentRow.GetCell(18).DateCellValue);
                                        break;
                                    case 19:
                                        c.Categoria = new ComboItem() { Data = currentRow.GetCell(19).StringCellValue };
                                        break;
                                    case 20:
                                        c.Tarea = new ComboItem() { Data = currentRow.GetCell(20).StringCellValue };
                                        break;
                                    case 21:
                                        c.Sindicato = new ComboItem() { Data = currentRow.GetCell(21).StringCellValue };
                                        break;
                                    case 22:
                                        c.ObraSocial = new ComboItem() { Data = currentRow.GetCell(22).StringCellValue };
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
                                    case 4:
                                        c.Email = null;
                                        break;
                                    case 5:
                                        c.Localizacion = null;
                                        break;
                                    case 6:
                                        c.Direccion = null;
                                        break;
                                    case 7:
                                        c.Numero = null;
                                        break;
                                    case 8:
                                        c.Departamento = null;
                                        break;
                                    case 9:
                                        c.Piso = null;
                                        break;
                                    case 10:
                                        c.CodigoPostal = null;
                                        break;
                                    case 11:
                                        c.Nacionalidad = null;
                                        break;
                                    case 12:
                                        c.EstadoCivil = null;
                                        break;
                                    case 13:
                                        c.Sexo = null;
                                        break;
                                    case 14:
                                        c.Telefono = null;
                                        break;
                                    case 15:
                                        c.SueldoBrutoMensual = null;
                                        break;
                                    case 16:
                                        c.SueldoBrutoHora = null;
                                        break;
                                    case 18:
                                        c.FechaNacimiento = null;
                                        break;
                                    case 19:
                                        c.Categoria = null;
                                        break;
                                    case 20:
                                        c.Tarea = null;
                                        break;
                                    case 21:
                                        c.Sindicato = null;
                                        break;
                                    case 22:
                                        c.ObraSocial = null;
                                        break;
                                    
                                }
                            }
                        }

                        //Campos obligatorios
                        c.CUIT = Cuit;
                        c.DNI = (currentRow.GetCell(1).CellType == CellType.String) ? currentRow.GetCell(1).StringCellValue : currentRow.GetCell(1).NumericCellValue.ToString();
                        c.Nombre = currentRow.GetCell(2).StringCellValue;
                        c.Apellido = currentRow.GetCell(3).StringCellValue;
                        c.FechaIngreso = Convert.ToDateTime(currentRow.GetCell(17).DateCellValue);
                        

                        //Campos no obligatorios
                        //c.Departamento = ((currentRow.GetCell(8) != null) && currentRow.GetCell(8).CellType == CellType.String) ? currentRow.GetCell(8).StringCellValue : currentRow.GetCell(8).NumericCellValue.ToString();
                        //c.Piso = ((currentRow.GetCell(9) != null) && currentRow.GetCell(9).CellType == CellType.String) ? currentRow.GetCell(9).StringCellValue : currentRow.GetCell(9).NumericCellValue.ToString();
                        //c.Telefono = ((currentRow.GetCell(14) != null) && currentRow.GetCell(14).CellType == CellType.String) ? currentRow.GetCell(14).StringCellValue : currentRow.GetCell(14).NumericCellValue.ToString();
                        //c.SueldoBrutoMensual =  (currentRow.GetCell(15) != null) ? Convert.ToDecimal(currentRow.GetCell(15).StringCellValue);
                        //c.SueldoBrutoHora = (currentRow.GetCell(15) != null) ? Convert.ToDecimal(currentRow.GetCell(16).StringCellValue);
                        //c.EstadoCivil = new ComboItem() { Data = currentRow.GetCell(12).StringCellValue };
                        //c.Sexo = new ComboItem() { Data = currentRow.GetCell(13).StringCellValue };
                        //c.Categoria = new ComboItem() { Data = currentRow.GetCell(19).StringCellValue };
                        //c.Tarea = new ComboItem() { Data = currentRow.GetCell(20).StringCellValue };

                        //c.Email = currentRow.GetCell(4).StringCellValue;
                        //c.Localizacion = new Localizacion() { Nombre = currentRow.GetCell(5).StringCellValue };
                        //c.Direccion = currentRow.GetCell(6).StringCellValue;
                        //c.Numero = (currentRow.GetCell(7).CellType == CellType.String) ? currentRow.GetCell(10).StringCellValue : currentRow.GetCell(10).NumericCellValue.ToString();
                        //c.CodigoPostal = (currentRow.GetCell(10).CellType == CellType.String) ? currentRow.GetCell(10).StringCellValue : currentRow.GetCell(10).NumericCellValue.ToString();
                        //c.Nacionalidad = new ComboItem() { Data = currentRow.GetCell(11).StringCellValue };
                        //c.FechaNacimiento = Convert.ToDateTime(currentRow.GetCell(18).DateCellValue);

                        
                        // Force celltypes
                        //for (int u = 5; u < 24; u++)
                        //{
                        //    if (currentRow.GetCell(u) != null)
                        //    {
                                
                        //        CellType type = currentRow.GetCell(u).CellType;
                        //        if(type == CellType.String)
                        //        {
                        //            currentRow.GetCell(u).SetCellType(CellType.String);
                        //        }
                        //        else if (type == CellType.Numeric)
                        //        {
                        //            currentRow.GetCell(u).SetCellType(CellType.Numeric);
                        //        }
                                
                        //    }
                        //}
                        //    currentRow.GetCell(5).SetCellType(CellType.String);
                        //currentRow.GetCell(6).SetCellType(CellType.String);
                        //currentRow.GetCell(7).SetCellType(CellType.String);
                        //currentRow.GetCell(8).SetCellType(CellType.String);
                        //currentRow.GetCell(9).SetCellType(CellType.String);
                        //currentRow.GetCell(10).SetCellType(CellType.String);
                        //currentRow.GetCell(11).SetCellType(CellType.String);
                        //currentRow.GetCell(12).SetCellType(CellType.String);
                        //currentRow.GetCell(13).SetCellType(CellType.String);
                        //currentRow.GetCell(14).SetCellType(CellType.String);
                        //currentRow.GetCell(15).SetCellType(CellType.String);
                        //currentRow.GetCell(16).SetCellType(CellType.String);
                        //currentRow.GetCell(17).SetCellType(CellType.String);
                        //currentRow.GetCell(18).SetCellType(CellType.String);
                        //currentRow.GetCell(19).SetCellType(CellType.String);
                        //currentRow.GetCell(20).SetCellType(CellType.String);
                        //currentRow.GetCell(21).SetCellType(CellType.String);
                        //currentRow.GetCell(22).SetCellType(CellType.String);
                        //currentRow.GetCell(23).SetCellType(CellType.String);

                        

                        // Model validations.
                        List<ValidationResult> validations;
                        bool isModelValid = new ModelValidatorHelper<Empleado>(c).Validate(out validations);
                        if (!isModelValid)
                        {
                            string message = String.Join("\n", validations.Select(x => x.ErrorMessage).ToArray());
                            throw new BusinessException(message);
                        }

                        this.SueldosConfigService.ImportEmpleado(c);
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
            string newFileName = "resultado_" + currentFileName;
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