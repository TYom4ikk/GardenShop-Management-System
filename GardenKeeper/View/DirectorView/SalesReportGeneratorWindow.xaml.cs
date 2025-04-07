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
                MessageBox.Show(model.CloseLogsExcel(), "Сохранение отчёта", MessageBoxButton.OK, MessageBoxImage.Information);
                model.CloseLogsExcel();
            }
            catch(Exception ex)
            {
            }
        }
    }
}
