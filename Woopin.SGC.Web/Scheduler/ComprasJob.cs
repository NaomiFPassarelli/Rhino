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
                    Cuit = cuitCell.StringCellValue;
                }

                if (currentRow != null && Cuit != null && Cuit.Length > 0)
                {
                    // Creates the comment cell
                    ICell statusCell = currentRow.CreateCell(13);
                    sheet.AutoSizeColumn(13);
                    statusCell.CellStyle.WrapText = true;

                    try
                    {
                        // Force celltypes
                        currentRow.GetCell(5).SetCellType(CellType.String);
                        currentRow.GetCell(6).SetCellType(CellType.String);
                        currentRow.GetCell(7).SetCellType(CellType.String);
                        currentRow.GetCell(8).SetCellType(CellType.String);
                        currentRow.GetCell(9).SetCellType(CellType.String);
                        currentRow.GetCell(10).SetCellType(CellType.String);
                        currentRow.GetCell(11).SetCellType(CellType.String);

                        Proveedor c = new Proveedor()
                        {
                            RazonSocial = currentRow.GetCell(1).StringCellValue,
                            CUIT = Cuit,
                            CategoriaIva = new CategoriaIVA() { Nombre = currentRow.GetCell(2).StringCellValue },
                            Email = currentRow.GetCell(3).StringCellValue,
                            Localizacion = new Localizacion() { Nombre = currentRow.GetCell(4).StringCellValue },
                            CodigoPostal = currentRow.GetCell(9).StringCellValue,
                            Direccion = currentRow.GetCell(5).StringCellValue,
                            Numero = currentRow.GetCell(6).StringCellValue,
                            Piso = currentRow.GetCell(8).StringCellValue,
                            Departamento = currentRow.GetCell(7).StringCellValue,
                            CondicionCompra = new ComboItem() { Data = currentRow.GetCell(12).StringCellValue },
                            Telefono = currentRow.GetCell(11).StringCellValue,
                            Localidad = currentRow.GetCell(10).StringCellValue
                        };

                        // Model validations.
                        List<ValidationResult> validations;
                        bool isModelValid = new ModelValidatorHelper<Proveedor>(c).Validate(out validations);
                        if (!isModelValid)
                        {
                            string message = String.Join("\n", validations.Select(x=>x.ErrorMessage).ToArray());
                            throw new BusinessException(message);
                        }

                        this.ComprasConfigService.ImportProveedor(c);
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