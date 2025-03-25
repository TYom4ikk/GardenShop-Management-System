using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace GardenKeeper.ViewModel
{
    class AuditLogReportGeneratorViewModel
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

            worksheet.Cells[1, 1].Value = "Пользователь";
            worksheet.Cells[1, 2].Value = "Id Товара";
            worksheet.Cells[1, 3].Value = "Название товара"; 
            worksheet.Cells[1, 4].Value = $"Действие";
            worksheet.Cells[1, 5].Value = $"Поле";
            worksheet.Cells[1, 6].Value = $"Дата";
            worksheet.Cells[1, 7].Value = $"Старое значение";
            worksheet.Cells[1, 8].Value = $"Новое значение";

            // Автоматически подгоняем ширину колонок под содержимое
            worksheet.Columns.AutoFit();
        }

        public void WriteLineLog(AuditLog log)
        {
            row++; column++;
            worksheet.Cells[row, column].Value = $"{log.Users.Email}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.Products.Id}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.Products.Name}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.Fields.Name}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.Fields.Name}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.ChangeDate}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.OldValue}";
            column++;
            worksheet.Cells[row, column].Value = $"{log.NewValue}";
            column = 0;
        }

        public string CloseLogsExcel()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"AuditLogReports\\log_{timestamp}.xlsx");

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            workbook.SaveAs(path);

            string answer = $"Отчёт сохранён в {path}";

            workbook.Close();
            excelApp.Quit();

            return answer;
        }
    }
}
