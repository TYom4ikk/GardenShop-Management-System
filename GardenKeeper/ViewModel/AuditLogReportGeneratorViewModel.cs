using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Diagnostics;

namespace GardenKeeper.ViewModel
{
    class AuditLogReportGeneratorViewModel
    {
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private int row;
        private int column;

        /// <summary>
        /// Открывает новый Excel файл для записи логов и настраивает заголовки столбцов
        /// </summary>
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

        /// <summary>
        /// Записывает одну строку лога в Excel файл
        /// </summary>
        /// <param name="log">Запись лога для записи</param>
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

        /// <summary>
        /// Предлагает пользователю выбрать путь для сохранения файла
        /// </summary>
        /// <returns>Выбранный путь или null, если пользователь отменил выбор</returns>
        private string GetSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                DefaultExt = "xlsx",
                FileName = $"audit_log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        /// <summary>
        /// Сохраняет и закрывает Excel файл с логами
        /// </summary>
        /// <returns>Путь к сохраненному файлу или null, если пользователь отменил сохранение</returns>
        public string CloseLogsExcel()
        {
            string path = GetSavePath();
            
            if (string.IsNullOrEmpty(path))
            {
                // Если пользователь отменил выбор, закрываем Excel и возвращаем null
                workbook.Close(false);
                excelApp.Quit();
                return null;
            }

            workbook.SaveAs(path);
            string answer = $"Отчёт сохранён в {path}";

            workbook.Close();
            excelApp.Quit();

            // Открываем файл в Excel
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });

            return answer;
        }

        /// <summary>
        /// Получает список всех пользователей
        /// </summary>
        /// <returns>Список пользователей</returns>
        public List<Users> GetUsers()
        {
            return Core.context.Users.ToList();
        }

        /// <summary>
        /// Получает список записей аудита для конкретного пользователя и даты
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="dateTime">Дата для фильтрации</param>
        /// <returns>Список записей аудита</returns>
        public List<AuditLog> GetAuditLogs(int userId, DateTime? dateTime)
        {
            return Core.context.AuditLog.Where(l => l.UserId == userId && l.ChangeDate == dateTime.Value).ToList();
        }
    }
}
