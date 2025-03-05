using GardenKeeper.View.UsersView.Partial;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GardenKeeper.View.UsersView
{
    /// <summary>
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();

            DataContext = new CatalogViewModel();
            var products = ((CatalogViewModel)DataContext).products;

            foreach(var product in products)
            {
                ProductCard card = new ProductCard(product);
                card.Width = 200;
                card.Height = 200;
                catalog.Children.Add(card);
            }
        }
    }
}
