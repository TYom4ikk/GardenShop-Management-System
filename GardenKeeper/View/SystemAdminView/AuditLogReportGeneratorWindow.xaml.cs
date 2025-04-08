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

namespace GardenKeeper.View.SystemAdminView
{
    /// <summary>
    /// Логика взаимодействия для AuditLogReportGeneratorWindow.xaml
    /// </summary>
    public partial class AuditLogReportGeneratorWindow : Window
    {
        AuditLogReportGeneratorViewModel model;
        Users chosenUser;
        DateTime? chosenDate;
        
        /// <summary>
        /// Инициализирует новый экземпляр окна генерации отчета аудита
        /// </summary>
        public AuditLogReportGeneratorWindow()
        {
            InitializeComponent();
            model = new AuditLogReportGeneratorViewModel();

            dateToLog.SelectedDate = DateTime.Now;

            userToLog.ItemsSource = model.GetUsers();
            userToLog.DisplayMemberPath = "Email";
            userToLog.SelectedValuePath = "Id";
            userToLog.SelectedIndex = 0;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку генерации отчета аудита
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void GenerateAuditLogReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chosenUser = model.GetUsers().FirstOrDefault(u => u.Id == (int)userToLog.SelectedValue);
                chosenDate = dateToLog.SelectedDate;

                model.OpenLogsExcel();
                foreach (var log in model.GetAuditLogs(chosenUser.Id, chosenDate))
                {
                    model.WriteLineLog(log);
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
