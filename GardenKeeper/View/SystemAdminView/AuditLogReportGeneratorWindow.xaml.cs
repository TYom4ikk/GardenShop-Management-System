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


        private void GenerateAuditLogReport_Click(object sender, RoutedEventArgs e)
        {
            chosenUser = model.GetUsers().FirstOrDefault(u => u.Id == (int)userToLog.SelectedValue);
            chosenDate = dateToLog.SelectedDate;

            model.OpenLogsExcel();
            foreach (var log in model.GetAuditLogs(chosenUser.Id, chosenDate))
            {
                model.WriteLineLog(log);
            }
            MessageBox.Show(model.CloseLogsExcel(), "Сохранение отчёта", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
