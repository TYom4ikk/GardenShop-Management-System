using System;
using GardenKeeper.Model;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GardenKeeperTests
{
    [TestClass]
    public class ShoppingCardTests
    {

        [TestMethod]
        public void ShoppingCardProductsInsertTest()
        {
            var products = new List<Products>
            {
                new Products { Id = 1, Name = "Product 1", PriceToSort = 100 },
                new Products { Id = 3, Name = "Product 3", PriceToSort = 300 }
            };
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products = products;
            var newProduct = new Products { Id = 2, Name = "Product 2", PriceToSort = 200 };
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Insert(1, newProduct);

            Assert.AreEqual(3, GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Count);
            Assert.AreEqual(1, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[0].Id);
            Assert.AreEqual(2, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[1].Id);
            Assert.AreEqual(3, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[2].Id);

            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Clear();
        }

        [TestMethod]
        public void ShoppingCardProductsRemoveTest()
        {
            var productToRemove = new Products { Id = 1, Name = "Product 1", PriceToSort = 100 };

            var products = new List<Products>
            {
                productToRemove,
                new Products { Id = 2, Name = "Product 2", PriceToSort = 200 },
                new Products { Id = 3, Name = "Product 3", PriceToSort = 300 }
            };
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products = products;
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Remove(productToRemove);

            Assert.AreEqual(2, GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Count); // Размер 2, а не 3
            Assert.AreEqual(2, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[0].Id);
            Assert.AreEqual(3, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[1].Id);

            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Clear();
        }


        [TestMethod]
        public void ShoppingCardProductsFindTest()
        {
            var productToFind = new Products { Id = 1, Name = "Product 1", PriceToSort = 100 };

            var products = new List<Products>
            {
                productToFind,
                new Products { Id = 2, Name = "Product 2", PriceToSort = 200 },
                new Products { Id = 3, Name = "Product 3", PriceToSort = 300 }
            };

            Assert.AreEqual(products.FirstOrDefault(p => p.Id == productToFind.Id), productToFind);
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Clear();
        }

        [TestMethod]
        public void ShoppingCardProductsRemoveByIndexTest()
        {
            var productToRemove = new Products { Id = 1, Name = "Product 1", PriceToSort = 100 };

            var products = new List<Products>
            {
                productToRemove,
                new Products { Id = 2, Name = "Product 2", PriceToSort = 200 },
                new Products { Id = 3, Name = "Product 3", PriceToSort = 300 }
            };
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products = products;
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.RemoveAt(0);

            Assert.AreEqual(2, GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Count); // Размер 2, а не 3
            Assert.AreEqual(2, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[0].Id);
            Assert.AreEqual(3, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[1].Id);

            Assert.AreEqual(products.FirstOrDefault(p => p.Id == productToRemove.Id), null);

            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Clear();
        }

        [TestMethod]
        public void ShoppingCardProductsReverseTest()
        {
            var products = new List<Products>
            {
                new Products { Id = 1, Name = "Product 1", PriceToSort = 100 },
                new Products { Id = 2, Name = "Product 2", PriceToSort = 200 },
                new Products { Id = 3, Name = "Product 3", PriceToSort = 300 }
            };
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products = products;
            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Reverse();

            Assert.AreEqual(3, GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Count);
            Assert.AreEqual(3, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[0].Id);
            Assert.AreEqual(2, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[1].Id);
            Assert.AreEqual(1, GardenKeeper.ViewModel.ShoppingCardViewModel.Products[2].Id);

            GardenKeeper.ViewModel.ShoppingCardViewModel.Products.Clear();
        }

    }
}
