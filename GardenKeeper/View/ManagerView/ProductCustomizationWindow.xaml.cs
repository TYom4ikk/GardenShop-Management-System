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

namespace GardenKeeper.View.ManagerView
{
    /// <summary>
    /// Логика взаимодействия для ProductCustomizationWindow.xaml
    /// </summary>
    public partial class ProductCustomizationWindow : Window
    {
        ProductCustomizationViewModel model;
        KeyValuePair<TextBox, TextBox> ProperyNameValue;
        public ProductCustomizationWindow(Products product)
        {
            InitializeComponent();
            DataContext = product;
            model = new ProductCustomizationViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //model.AddNewProperty();
        }

        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PropertyName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PropertyValue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
