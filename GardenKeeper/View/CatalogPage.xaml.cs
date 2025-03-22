using GardenKeeper.Model;
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
        private const int MANAGER_TYPE_ID = 2;

        private CatalogViewModel viewModel;
        private Users currentUser = null;
        private bool isRegisteredUser;
        private bool isManager;
        public CatalogPage(Users user)
        {
            InitializeComponent();
            viewModel = new CatalogViewModel(user);
            DataContext = viewModel;

            currentUser = user;
            isRegisteredUser = user.Email != null ? true : false;
            isManager = user.UserTypeId == MANAGER_TYPE_ID ? true : false;

            PriceFilterComboBox.ItemsSource = Enum.GetValues(typeof(CatalogViewModel.PriceFilterStatuses));
            PriceFilterComboBox.SelectedIndex = 0;

            CategoriesFilterComboBox.ItemsSource = viewModel.Categories;
            CategoriesFilterComboBox.DisplayMemberPath = viewModel.CategoriesFilterDisplayMemberPath;
            CategoriesFilterComboBox.SelectedIndex = 0;

            LoginButton.Content = isRegisteredUser ? "Войти в другой аккаунт" : "Войти в аккаунт";

            UpdateProductDisplay(viewModel.Products);
        }


        private void UpdateProductDisplay(List<Products> products)
        {
            CatalogUniformGrid.Children.Clear();

            foreach (var product in products)
            {
                ProductCard card = new ProductCard(product, isRegisteredUser, isManager, currentUser)
                {
                    Width = 400,
                    Height = 500,
                    Margin = new Thickness(0, 20, 0, 20)
                };
                CatalogUniformGrid.Children.Add(card);
            }
        }

        private void PriceFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void CategoriesFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            if(PriceFilterComboBox.SelectedItem is CatalogViewModel.PriceFilterStatuses selectedFilter &&
                CategoriesFilterComboBox.SelectedItem is Categories category)
            {
                UpdateProductDisplay(viewModel.PriceFilter(selectedFilter).ToList());
                UpdateProductDisplay(viewModel.CategoryFilter(category.Id).ToList());
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage page = new RegistrationPage();
            this.NavigationService.Navigate(page);
        }

        private void ShoppingCardButton_Click(object sender, RoutedEventArgs e)
        {
            ShoppingCardPage page = new ShoppingCardPage(currentUser);
            this.NavigationService.Navigate(page);
        }
    }
}
