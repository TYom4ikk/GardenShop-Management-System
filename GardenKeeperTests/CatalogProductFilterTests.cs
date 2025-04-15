using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GardenKeeper.Model;
using GardenKeeper.ViewModel;
using System.Linq;
using System.Collections.Generic;
using System.Security.Policy;
using GardenKeeper;

namespace GardenKeeperTests
{
    [TestClass]
    public class CatalogProductFilterTests
    {
        [TestMethod]
        public void FilterAllCategoriesTest()
        {
            var viewModel = new CatalogViewModel();
            var products = new List<Products>
            {
                new Products { Id = 1, Name = "Product1", PriceToSort = 100, CategoryId = 1 },
                new Products { Id = 2, Name = "Product2", PriceToSort = 50, CategoryId = 2  },
                new Products { Id = 3, Name = "Product3", PriceToSort = 200, CategoryId = 1 },
                new Products { Id = 4, Name = "Product3", PriceToSort = 200, CategoryId = 3 }
            };

            viewModel.Products = products;
            var sortedProducts = viewModel.CategoryFilter(1);

            Assert.AreEqual(4, sortedProducts.Count()); // categoryId = 1 - это родительская категорий,                                    // всех категорий
        }

        [TestMethod]
        public void FilterByCategoryTest()
        {
            var viewModel = new CatalogViewModel();
            var products = new List<Products>
            {
                new Products { Id = 1, Name = "Product1", PriceToSort = 100, CategoryId = 2 },
                new Products { Id = 2, Name = "Product2", PriceToSort = 50, CategoryId = 2  },
                new Products { Id = 3, Name = "Product3", PriceToSort = 200, CategoryId = 1 },
                new Products { Id = 4, Name = "Product3", PriceToSort = 200, CategoryId = 3 }
            };

            viewModel.Products = products;
            var sortedProducts = viewModel.CategoryFilter(2);

            Assert.AreEqual(2, sortedProducts.Count()); 
        }

        [TestMethod]
        public void FilterByPriceByASCTest()
        {
            var viewModel = new CatalogViewModel();
            var products = new List<Products>
            {
                new Products { Id = 1, Name = "Product1", MainPrice = 10000, CategoryId = 2 },
                new Products { Id = 2, Name = "Product2", MainPrice= 5000, CategoryId = 2  },
                new Products { Id = 3, Name = "Product3", MainPrice = 50000, CategoryId = 1 },
                new Products { Id = 4, Name = "Product3", MainPrice = 20000, CategoryId = 3 }
            };
            viewModel.Products = products;

            List<Products> sortedProducts = viewModel.PriceFilter(CatalogViewModel.PriceFilterStatuses.Дороже, products).ToList();

            Assert.AreEqual(50000, sortedProducts[0].PriceToSort);
            Assert.AreEqual(3, sortedProducts[0].Id);
        }
        [TestMethod]
        public void FilterByPriceDESCTest()
        {
            var viewModel = new CatalogViewModel();
            var products = new List<Products>
            {
                new Products { Id = 1, Name = "Product1", MainPrice = 10000, CategoryId = 2 },
                new Products { Id = 2, Name = "Product2", MainPrice= 5000, CategoryId = 2  },
                new Products { Id = 3, Name = "Product3", MainPrice = 50000, CategoryId = 1 },
                new Products { Id = 4, Name = "Product3", MainPrice = 20000, CategoryId = 3 }
            };
            viewModel.Products = products;

            List<Products> sortedProducts = viewModel.PriceFilter(CatalogViewModel.PriceFilterStatuses.Дешевле, products).ToList();

            Assert.AreEqual(5000, sortedProducts[0].PriceToSort);
            Assert.AreEqual(2, sortedProducts[0].Id);
        }
    }
}
