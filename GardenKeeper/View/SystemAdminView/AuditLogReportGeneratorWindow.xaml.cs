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

            userToLog.ItemsSource = Core.context.Users.ToList();
            userToLog.DisplayMemberPath = "Email";
            userToLog.SelectedValuePath = "Id";
            userToLog.SelectedIndex = 0;
        }


        private void GenerateAuditLogReport_Click(object sender, RoutedEventArgs e)
        {
            chosenUser = Core.context.Users.FirstOrDefault(u => u.Id == (int)userToLog.SelectedValue);
            chosenDate = dateToLog.SelectedDate;

            model.OpenLogsExcel();
            foreach (var log in Core.context.AuditLog.Where(l => l.UserId == chosenUser.Id && l.ChangeDate == chosenDate.Value))
            {
                model.WriteLineLog(log);
            }
            MessageBox.Show(model.CloseLogsExcel(), "Сохранение отчёта", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
