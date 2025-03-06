using GardenKeeper.Model;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GardenKeeper.View
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            //Обработать cancel
            byte[]image_bytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);

            var product = Core.context.Products.Where(p => p.Id == 3).FirstOrDefault();
            product.Image = image_bytes;
            Core.context.SaveChanges();
        }
    }
}
