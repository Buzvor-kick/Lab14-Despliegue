using ClosedXML.Excel;
using Lab11_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab11_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly LinqexampleContext _context;

        public ReportesController(LinqexampleContext context)
        {
            _context = context;
        }

        [HttpGet("descargar-productos")]
        public async Task<IActionResult> DescargarProductosExcel()
        {
            // Obtener los productos de la base de datos MySQL
            var productos = await _context.Products.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Productos");

                // 1. Crear encabezados con estilo (3 columnas: A, B, C)
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nombre";
                worksheet.Cell(1, 3).Value = "Precio";

                var rangoEncabezados = worksheet.Range("A1:C1");
                rangoEncabezados.Style.Font.Bold = true;
                rangoEncabezados.Style.Fill.BackgroundColor = XLColor.LightSkyBlue;

                // 2. Llenar las filas con los datos de MySQL
                int fila = 2;
                foreach (var prod in productos)
                {
                    worksheet.Cell(fila, 1).Value = prod.ProductId;
                    worksheet.Cell(fila, 2).Value = prod.Name;
                    worksheet.Cell(fila, 3).Value = prod.Price;
                    fila++;
                }

                // Ajustar el ancho de las columnas automáticamente
                worksheet.Columns().AdjustToContents();

                // 3. Guardar en memoria y devolver como archivo descargable
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Reporte_Productos.xlsx"
                    );
                }
            }
        }
    }
}