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

        /// <summary>
        /// Инициализирует состояние страницы каталога
        /// </summary>
        /// <param name="user">Текущий пользователь</param>
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

            SpecialPanel.Visibility = Visibility.Hidden;
            SpecialPanelSplitter.Visibility = Visibility.Hidden;
            GenerateSalesReportButton.Visibility = Visibility.Hidden;


            if (isDirector || isAdmin)
            {
                SpecialPanel.Visibility = Visibility.Visible;
                SpecialPanelSplitter.Visibility = Visibility.Visible;

                if (isDirector)
                {
                    GenerateSalesReportButton.Visibility = Visibility.Visible;
                }
            }

            LoginButton.Content = isRegisteredUser ? "Выйти" : "Войти в аккаунт";


            

            if (!isManager)
            {
                CatalogContent.ColumnDefinitions.Clear();
                CatalogContent.ColumnDefinitions.Add(new ColumnDefinition());
            }
            UpdateProductDisplay(viewModel.Products);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса CatalogPage для неавторизованного пользователя
        /// </summary>
        public CatalogPage()
        {
            InitializeComponent();
            Users user = new Users();
            user.UserTypeId = 1;
            ConstructorBody(user);
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса CatalogPage для указанного пользователя
        /// </summary>
        /// <param name="user">Пользователь, для которого создается страница</param>
        public CatalogPage(Users user)
        {
            InitializeComponent();
            ConstructorBody(user);
        }
       
        /// <summary>
        /// Обновляет отображение продуктов в каталоге
        /// </summary>
        /// <param name="products">Список продуктов для отображения</param>
        /// <returns>Список отображаемых продуктов</returns>
        private List<Products> UpdateProductDisplay(List<Products> products)
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
            return products;
        }

        /// <summary>
        /// Обработчик изменения фильтра по цене
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void PriceFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Обработчик изменения фильтра по категории
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void CategoriesFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Применяет текущие фильтры к списку продуктов
        /// </summary>
        private void Filter()
        {
            if(PriceFilterComboBox.SelectedItem is CatalogViewModel.PriceFilterStatuses selectedFilter &&
                CategoriesFilterComboBox.SelectedItem is Categories category)
            {
                var sortedProducts = viewModel.CategoryFilter(category.Id).ToList();
                UpdateProductDisplay(viewModel.PriceFilter(selectedFilter, sortedProducts).ToList());
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку входа/выхода
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage page = new RegistrationPage();
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку корзины
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void ShoppingCardButton_Click(object sender, RoutedEventArgs e)
        {
            ShoppingCardPage page = new ShoppingCardPage(currentUser);
            this.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку генерации отчета аудита
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void GenerateAuditLogReportButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuditLogReportGeneratorWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку добавления нового менеджера
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void AddNewManagerButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddNewManagerWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Обработчик нажатия на кнопку генерации отчета о продажах
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        private void GenerateSalesReportButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new SalesReportGeneratorWindow();
            window.ShowDialog();
        }
    }
}
