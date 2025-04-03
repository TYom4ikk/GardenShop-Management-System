using GardenKeeper.Model;
using GardenKeeper.View.DirectorView;
using GardenKeeper.View.SystemAdminView;
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
        private const int ADMIN_TYPE_ID = 3;
        private const int DIRECTOR_TYPE_ID = 4;

        private CatalogViewModel viewModel;
        private Users currentUser = null;
        private bool isRegisteredUser;
        private bool isManager;
        private bool isAdmin;
        private bool isDirector;

        public void ConstructorBody(Users user)
        {
            viewModel = new CatalogViewModel(user);
            DataContext = viewModel;

            currentUser = user;
            isRegisteredUser = user.Email != null ? true : false;
            isManager = user.UserTypeId == MANAGER_TYPE_ID ? true : false;
            isAdmin = user.UserTypeId == ADMIN_TYPE_ID ? true : false;
            isDirector = user.UserTypeId == DIRECTOR_TYPE_ID ? true : false;

            if (isDirector) { isAdmin = true; }
            if (isAdmin) { isManager = true; }

            PriceFilterComboBox.ItemsSource = Enum.GetValues(typeof(CatalogViewModel.PriceFilterStatuses));
            PriceFilterComboBox.SelectedIndex = 0;

            CategoriesFilterComboBox.ItemsSource = viewModel.Categories;
            CategoriesFilterComboBox.DisplayMemberPath = viewModel.CategoriesFilterDisplayMemberPath;
            CategoriesFilterComboBox.SelectedIndex = 0;

            LoginButton.Content = isRegisteredUser ? "Войти в другой аккаунт" : "Войти в аккаунт";

            SpecialPanel.Visibility = isManager ? Visibility.Visible : Visibility.Hidden;
            SpecialPanelSplitter.Visibility = isManager ? Visibility.Visible : Visibility.Hidden;

            if (!isManager)
            {
                CatalogContent.ColumnDefinitions.Clear();
                CatalogContent.ColumnDefinitions.Add(new ColumnDefinition());
            }
            UpdateProductDisplay(viewModel.Products);
        }

        public CatalogPage()
        {
            InitializeComponent();
            Users user = new Users();
            user.UserTypeId = 1;
            ConstructorBody(user);
        }
        public CatalogPage(Users user)
        {
            InitializeComponent();
            ConstructorBody(user);
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
                UpdateProductDisplay(viewModel.CategoryFilter(category.Id).ToList());
                UpdateProductDisplay(viewModel.PriceFilter(selectedFilter).ToList());
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

        private void GenerateAuditLogReportButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuditLogReportGeneratorWindow();
            window.ShowDialog();
        }

        private void AddNewManagerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddNewManagerWindow();
            window.ShowDialog();
        }

        private void GenerateSalesReportButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SalesReportGeneratorWindow();
            window.ShowDialog();
        }
    }
}
