using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.ViewModel
{
    class SaleViewModel
    {
        public List<Sales> Sales{ get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса SaleViewModel и загружает все продажи
        /// </summary>
        public SaleViewModel()
        {
            Core.context.Sales.ToList();
        }

        /// <summary>
        /// Добавляет новую продажу
        /// </summary>
        /// <param name="productId">Идентификатор товара</param>
        /// <param name="quantity">Количество товара</param>
        public void AddSale(int productId, long quantity)
        {
            Sales sale = new Sales
            {
                Quantity = 1,
                SaleDate = DateTime.Now,
                TotalPrice = 0,
                UnitPrice = 0,
            };
        }
    }
}
