using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Diagnostics;

namespace GardenKeeper.ViewModel
{
    class SalesReportGeneratorViewModel
    {
        private Excel.Application excelApp;
        private Excel.Workbook workbook;
        private Excel.Worksheet worksheet;
        private int row;
        private int column;

        /// <summary>
        /// Открывает новый Excel файл для записи отчета о продажах и настраивает заголовки столбцов
        /// </summary>
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

        /// <summary>
        /// Записывает одну строку продажи в Excel файл
        /// </summary>
        /// <param name="sale">Запись о продаже для записи</param>
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
                FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return null;
        }

        /// <summary>
        /// Сохраняет и закрывает Excel файл с отчетом о продажах
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
        /// Получает список продаж за указанный год
        /// </summary>
        /// <param name="year">Год для фильтрации</param>
        /// <returns>Список продаж за указанный год</returns>
        public IEnumerable<Sales> SalesByYear(int year)
        {
            return Core.context.Sales.Where(s => s.SaleDate.Year == year);
        }

        /// <summary>
        /// Получает список продаж за указанный месяц и год
        /// </summary>
        /// <param name="year">Год для фильтрации</param>
        /// <param name="month">Месяц для фильтрации</param>
        /// <returns>Список продаж за указанный месяц и год</returns>
        public IEnumerable<Sales> SalesByMonth(int year, int month)
        {
            return Core.context.Sales.Where(s => s.SaleDate.Year == year && s.SaleDate.Month == month);
        }
    }
}
