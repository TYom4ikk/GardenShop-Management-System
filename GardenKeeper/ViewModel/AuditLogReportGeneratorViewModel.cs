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
    class AuditLogReportGeneratorViewModel
    {


        public void WriteLogToExcel(AuditLog log, DateTime dateTime, Products product)
        {
            // Инициализация Excel приложения
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;  // Если нужно, можно установить в true, чтобы видеть Excel

            // Создание новой книги
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];  // Доступ к первому листу

            // Запись данных в ячейку A1
            worksheet.Cells[1, 1].Value = "Привет, мир!";

            // Сохранение файла
            workbook.SaveAs("C:\\path\\to\\your\\file.xlsx");

            // Закрытие
            workbook.Close();
            excelApp.Quit();
        }
        public string WriteLogToExcel(AuditLog log, Users user, Products product)
        {
            // Инициализация Excel приложения
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;

            // Создание нового рабочего файла
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            // Заполнение ячеек
            worksheet.Cells[1, 1].Value = "Пользователь";
            worksheet.Cells[2, 1].Value = $"{user.Email}";

            worksheet.Cells[1, 2].Value = "Id Товара";
            worksheet.Cells[2, 2].Value = $"{product.Id}";

            worksheet.Cells[1, 3].Value = "Название товара";
            worksheet.Cells[2, 3].Value = $"{product.Name}";

            // Генерация уникального имени файла на основе текущей даты и времени
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"AuditLogReports\\log_{timestamp}.xlsx");

            // Создание директории, если она не существует
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Сохранение нового файла
            workbook.SaveAs(path);

            // Ответ с информацией о пути сохранения файла
            string answer = $"Отчёт сохранён в {path}";

            // Закрытие рабочего файла и приложения Excel
            workbook.Close();
            excelApp.Quit();

            return answer;
        }
    }
}
