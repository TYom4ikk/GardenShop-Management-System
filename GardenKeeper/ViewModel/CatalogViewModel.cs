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
            Default,
            Cheap,
            Expensive,
            Date
        }
        public PriceFilterStatuses PriceFilterStatus = PriceFilterStatuses.Default;
        public string CategoriesFilterDisplayMemberPath = "Name";
        public List<Products> Products { get; set; }
        public List<Categories> Categories { get; set; }
        public Users currentUser { get; set; }
        public CatalogViewModel(Users user)
        {
            Products = Core.context.Products.ToList();
            Categories = Core.context.Categories.ToList();
            currentUser = user;
        }

        public IEnumerable<Products> PriceFilter(PriceFilterStatuses selectedFilter)
        {
            IEnumerable<Products> filteredProducts;
            switch (selectedFilter)
            {
                case PriceFilterStatuses.Cheap:
                    filteredProducts = Products.OrderBy(p => p.PriceToSort);
                    break;
                case PriceFilterStatuses.Expensive:
                    filteredProducts = Products.OrderByDescending(p => p.PriceToSort);
                    break;
                case PriceFilterStatuses.Date:
                    filteredProducts = Products.OrderBy(p => p.PriceToSort);
                    break;
                case PriceFilterStatuses.Default:
                    filteredProducts = Products;
                    break;
                default:
                    filteredProducts = Products;
                    break;
            }
            return filteredProducts;
        }
        public IEnumerable<Products> CategoryFilter(int categoryId)
        {
            return Products.Where(p=>p.CategoryId == categoryId);
        }
    }
}
