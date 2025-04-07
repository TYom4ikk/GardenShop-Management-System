using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace GardenKeeper.ViewModel
{
    class SalesReportGeneratorViewModel
    {
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private int row;
        private int column;
        public void OpenLogsExcel()
        {
            excelApp = new Excel.Application();
            excelApp.Visible = false;

            workbook = excelApp.Workbooks.Add();
            worksheet = (Excel.Worksheet)workbook.Sheets[1];

            row = 1;
            column = 0;

            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Id Товара";
            worksheet.Cells[1, 3].Value = "Название товара";
            worksheet.Cells[1, 4].Value = $"Количество";
            worksheet.Cells[1, 5].Value = $"Дата продажи";
            worksheet.Cells[1, 6].Value = $"Цена за штуку";
            worksheet.Cells[1, 7].Value = $"Итоговая цена";
            worksheet.Cells[1, 8].Value = $"Покупатель";
            worksheet.Columns.AutoFit();
        }

        public void WriteLineLog(Sales sale)
        {
            row++; column++;
            worksheet.Cells[row, column].Value = $"{sale.Id}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.Products.Id}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.Products.Name}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.Quantity}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.SaleDate}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.UnitPrice}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.TotalPrice}";
            column++;
            worksheet.Cells[row, column].Value = $"{sale.Users.Email}";
            column = 0;
        }

        public string CloseLogsExcel()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"SaleReports\\log_{timestamp}.xlsx");

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            workbook.SaveAs(path);

            string answer = $"Отчёт сохранён в {path}";

            workbook.Close();
            excelApp.Quit();

            return answer;
        }
        public IEnumerable<Sales> SalesByYear(int year)
        {
            return Core.context.Sales.Where(s => s.SaleDate.Year == year);
        }
        public IEnumerable<Sales> SalesByMonth(int year, int month)
        {
            return Core.context.Sales.Where(s => s.SaleDate.Year == year && s.SaleDate.Month == month);
        }
    }
}
