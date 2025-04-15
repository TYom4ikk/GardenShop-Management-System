using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.ViewModel
{
    public class CatalogViewModel
    {
        public enum PriceFilterStatuses
        {
            Стандарт,
            Дешевле,
            Дороже,
            Дата
        }
        public PriceFilterStatuses PriceFilterStatus = PriceFilterStatuses.Стандарт;
        public string CategoriesFilterDisplayMemberPath = "Name";
        public List<Products> Products { get; set; }
        public List<Categories> Categories { get; set; }
        public Users currentUser { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса CatalogViewModel
        /// </summary>
        /// <param name="user">Текущий пользователь</param>
        public CatalogViewModel(Users user)
        {
            Products = Core.context.Products.ToList();
            Categories = Core.context.Categories.ToList();
            currentUser = user;
        }

        public CatalogViewModel()
        {
            
        }

        /// <summary>
        /// Фильтрует список товаров по цене в соответствии с выбранным статусом
        /// </summary>
        /// <param name="selectedFilter">Выбранный статус фильтрации</param>
        /// <param name="products">Список товаров для фильтрации</param>
        /// <returns>Отфильтрованный список товаров</returns>
        public IEnumerable<Products> PriceFilter(PriceFilterStatuses selectedFilter, List<Products> products)
        {
            IEnumerable<Products> filteredProducts;
            switch (selectedFilter)
            {
                case PriceFilterStatuses.Дешевле:
                    filteredProducts = products.Where(p => p.PriceToSort.HasValue)
                                               .OrderBy(p => p.PriceToSort);
                    break;
                case PriceFilterStatuses.Дороже:
                    filteredProducts = products.Where(p => p.PriceToSort.HasValue)
                                               .OrderByDescending(p => p.PriceToSort);
                    break;
                case PriceFilterStatuses.Дата:
                    filteredProducts = products.Where(p => p.PriceToSort.HasValue)
                                               .OrderBy(p => p.PriceToSort);
                    break;
                case PriceFilterStatuses.Стандарт:
                    filteredProducts = products;
                    break;
                default:
                    filteredProducts = products;
                    break;
            }
            return filteredProducts;
        }

        /// <summary>
        /// Фильтрует список товаров по категории
        /// </summary>
        /// <param name="categoryId">Идентификатор категории</param>
        /// <returns>Отфильтрованный список товаров</returns>
        public IEnumerable<Products> CategoryFilter(int categoryId)
        {
            if(categoryId == 1)
            {
                return Products;
            }
            return Products.Where(p=>p.CategoryId == categoryId);
        }
    }
}
