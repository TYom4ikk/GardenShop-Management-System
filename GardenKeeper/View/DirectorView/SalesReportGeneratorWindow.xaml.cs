using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GardenKeeper.View.DirectorView
{
    /// <summary>
    /// Формирование отчётов о продажах
    /// </summary>
    public partial class SalesReportGeneratorWindow : Window
    {
        SalesReportGeneratorViewModel model;

        /// <summary>
        /// Инициализирует новый экземпляр окна генерации отчета о продажах
        /// </summary>
        public SalesReportGeneratorWindow()
        {
            InitializeComponent();

            model = new SalesReportGeneratorViewModel();

            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 5; year <= currentYear; year++)
            {
                YearComboBox.Items.Add(year);
            }
            YearComboBox.SelectedItem = currentYear;

            string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                              "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            MonthComboBox.ItemsSource = months;
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        /// <summary>
        /// Обработчик изменения состояния радиокнопок выбора периода
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (MonthComboBox != null)
            {
                if (YearRadioButton.IsChecked == true)
                {
                    MonthComboBox.SelectedItem = null;
                    MonthComboBox.IsEnabled = false;
                }
                else
                {
                    MonthComboBox.IsEnabled = true;
                    MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку генерации отчета о продажах
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void GenerateSalesReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var year = (int)YearComboBox.SelectedItem;
                var monthStr = (string)MonthComboBox.SelectedItem;
                var month = Array.IndexOf(new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                                                   "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" },
                                        monthStr) + 1;

                model.OpenLogsExcel();
                if (YearRadioButton.IsChecked == true)
                {
                    foreach (var sale in model.SalesByYear(year))
                    {
                        model.WriteLineLog(sale);
                    }
                }
                else if (MonthRadioButton.IsChecked == true)
                {
                    foreach (var sale in model.SalesByMonth(year, month))
                    {
                        model.WriteLineLog(sale);
                    }
                }
                string result = model.CloseLogsExcel();
                if (result != null)
                {
                    MessageBox.Show(result, "Сохранение отчёта", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка при формировании отчета: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
