using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.ViewModel
{
    public class ShoppingCardViewModel
    {
        public static List<Products> Products = new List<Products>();
        /// <summary>
        /// Добавление продажи в базу данных
        /// </summary>
        /// <param name="sale"></param>
        public void AddSale(Sales sale)
        {
            Core.context.Sales.Add(sale);
            Core.context.SaveChanges();
        }
    }
}
