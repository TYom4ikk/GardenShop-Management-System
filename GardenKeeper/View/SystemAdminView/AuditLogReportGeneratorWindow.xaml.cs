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
        public AuditLogReportGeneratorWindow()
        {
            InitializeComponent();
            model = new AuditLogReportGeneratorViewModel();
            var log = Core.context.AuditLog.FirstOrDefault();
            var user = Core.context.Users.FirstOrDefault(u=>u.Id==log.UserId);
            var product = Core.context.Products.FirstOrDefault(u=>u.Id==log.ProductId);
            model.WriteLogToExcel(log, user, product);
        }
    }
}
