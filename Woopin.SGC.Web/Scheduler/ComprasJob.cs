using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Compras;
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
    public class ComprasJobs
    {
        private readonly IComprasService ComprasService;
        private readonly IComprasReportService ComprasReportService;
        private readonly ISystemService SystemService;
        private readonly IComprasConfigService ComprasConfigService;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ComprasJobs(IComprasConfigService ComprasConfigService, IComprasService ComprasService, IComprasReportService ComprasReportService, ISystemService SystemService)
        {
            this.ComprasService = ComprasService;
            this.ComprasReportService = ComprasReportService;
            this.SystemService = SystemService;
            this.ComprasConfigService = ComprasConfigService;
        }

        public void ImportarProveedores(string path, JobHeader jobHeader)
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

            ISheet sheet = hssfwb.GetSheet("Proveedores");
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                IRow currentRow = sheet.GetRow(row);

                ICell cuitCell = currentRow.GetCell(0);
                string Cuit = null;
                if (cuitCell != null)
                {
                    Cuit = (currentRow.GetCell(0).CellType == CellType.String) ? currentRow.GetCell(0).StringCellValue : currentRow.GetCell(0).NumericCellValue.ToString();
                    //Cuit = cuitCell.StringCellValue;
                }

                if (currentRow != null && Cuit != null && Cuit.Length > 0)
                {
                    // Creates the comment cell
                    ICell statusCell = currentRow.CreateCell(15);
                    sheet.AutoSizeColumn(15);
                    statusCell.CellStyle.WrapText = true;

                    try
                    {
                        // Force celltypes
                        //currentRow.GetCell(5).SetCellType(CellType.String);
                        //currentRow.GetCell(6).SetCellType(CellType.String);
                        //currentRow.GetCell(7).SetCellType(CellType.String);
                        //currentRow.GetCell(8).SetCellType(CellType.String);
                        //currentRow.GetCell(9).SetCellType(CellType.String);
                        //currentRow.GetCell(10).SetCellType(CellType.String);
                        //currentRow.GetCell(11).SetCellType(CellType.String);

                        //Proveedor c = new Proveedor()
                        //{
                        //    RazonSocial = currentRow.GetCell(1).StringCellValue,
                        //    CUIT = Cuit,
                        //    CategoriaIva = new CategoriaIVA() { Nombre = currentRow.GetCell(2).StringCellValue },
                        //    Email = currentRow.GetCell(3).StringCellValue,
                        //    Direccion = currentRow.GetCell(4).StringCellValue,
                        //    Numero = currentRow.GetCell(5).StringCellValue,
                        //    Piso = currentRow.GetCell(6).StringCellValue,
                        //    Departamento = currentRow.GetCell(7).StringCellValue,
                        //    CodigoPostal = currentRow.GetCell(8).StringCellValue,
                        //    Localidad = new Localidad() { Nombre = currentRow.GetCell(9).StringCellValue },
                        //    Localizacion = new Localizacion() { Nombre = currentRow.GetCell(10).StringCellValue },
                        //    Pais = new ComboItem() { Data = currentRow.GetCell(11).StringCellValue },
                        //    Telefono = currentRow.GetCell(12).StringCellValue,
                        //    CondicionCompra = new ComboItem() { Data = currentRow.GetCell(13).StringCellValue },
                        //};

                        Proveedor p = new Proveedor() { };
                        int[] obligatorios = new int[] { 1, 2, 13};
                        int columnsExcel = 14;
                        for (int u = 1; u < columnsExcel; u++)
                        {
                            //if (currentRow.GetCell(u) != null && currentRow.GetCell(u).ToString() != "" )
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

                                //Campos NO obligatorios
                                switch (u)
                                {
                                    case 3:
                                        p.Email = (currentRow.GetCell(3).CellType == CellType.String) ? currentRow.GetCell(3).StringCellValue : currentRow.GetCell(3).NumericCellValue.ToString();
                                        break;
                                    case 4:
                                        p.Direccion = (currentRow.GetCell(4).CellType == CellType.String) ? currentRow.GetCell(4).StringCellValue : currentRow.GetCell(4).NumericCellValue.ToString();
                                        break;
                                    case 5:
                                        p.Numero = (currentRow.GetCell(5).CellType == CellType.String) ? currentRow.GetCell(5).StringCellValue : currentRow.GetCell(5).NumericCellValue.ToString();
                                        break;
                                    case 6:
                                        p.Piso = (currentRow.GetCell(6).CellType == CellType.String) ? currentRow.GetCell(6).StringCellValue : currentRow.GetCell(6).NumericCellValue.ToString();
                                        break;
                                    case 7:
                                        p.Departamento = (currentRow.GetCell(7).CellType == CellType.String) ? currentRow.GetCell(7).StringCellValue : currentRow.GetCell(7).NumericCellValue.ToString();
                                        break;
                                    case 8:
                                        p.CodigoPostal = (currentRow.GetCell(8).CellType == CellType.String) ? currentRow.GetCell(8).StringCellValue : currentRow.GetCell(8).NumericCellValue.ToString();
                                        break;
                                    case 9:
                                        p.Localidad = new Localidad() { Nombre = currentRow.GetCell(9).StringCellValue };
                                        break;
                                    case 10:
                                        p.Localizacion = new Localizacion() { Nombre = currentRow.GetCell(10).StringCellValue };
                                        break;
                                    case 11:
                                        p.Pais = new ComboItem() { Data = currentRow.GetCell(11).StringCellValue };
                                        break;
                                    case 12:
                                        p.Telefono = (currentRow.GetCell(12).CellType == CellType.String) ? currentRow.GetCell(12).StringCellValue : currentRow.GetCell(12).NumericCellValue.ToString();
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
                                switch (u)
                                {
                                    case 3:
                                        p.Email = null;
                                        break;
                                    case 4:
                                        p.Direccion =null;
                                        break;
                                    case 5:
                                        p.Numero = null;
                                        break;
                                    case 6:
                                        p.Piso = null;
                                        break;
                                    case 7:
                                        p.Departamento = null;
                                        break;
                                    case 8:
                                        p.CodigoPostal = null;
                                        break;
                                    case 9:
                                        p.Localidad = null;
                                        break;
                                    case 10:
                                        p.Localizacion = null;
                                        break;
                                    case 11:
                                        p.Pais = null;
                                        break;
                                    case 12:
                                        p.Telefono = null;
                                        break;
                                }

                            }
                        }

                        //Campos obligatorios
                        p.CUIT = Cuit;
                        p.RazonSocial = (currentRow.GetCell(1).CellType == CellType.String) ? currentRow.GetCell(1).StringCellValue : currentRow.GetCell(1).NumericCellValue.ToString();
                        p.CategoriaIva = new CategoriaIVA() { Nombre = currentRow.GetCell(2).StringCellValue };
                        p.CondicionCompra = new ComboItem() { Data = currentRow.GetCell(13).StringCellValue };
                        
                        // Model validations.
                        List<ValidationResult> validations;
                        bool isModelValid = new ModelValidatorHelper<Proveedor>(p).Validate(out validations);
                        if (!isModelValid)
                        {
                            string message = String.Join("\n", validations.Select(x=>x.ErrorMessage).ToArray());
                            throw new BusinessException(message);
                        }

                        this.ComprasConfigService.ImportProveedor(p);
                        statusCell.SetCellValue("Importado");
                    }
                    catch (BusinessException be)
                    {
                        statusCell.SetCellValue(be.ErrorMessage);
                    }
                    catch(Exception e)
                    {
                        string msg = "Ocurrio un error inesperado al importar. Vuelva a intentarlo.";
                        statusCell.SetCellValue(msg);
                    }

                }
            }
            string currentFileName = Path.GetFileName(path);
            string newFileName = "resultado_proveedores_" + currentFileName;
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