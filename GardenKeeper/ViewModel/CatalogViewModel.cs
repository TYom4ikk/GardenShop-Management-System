using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.ViewModel
{
    public  class CatalogViewModel
    {
        public List<Products> products;
        public CatalogViewModel()
        {
            products = Core.context.Products.ToList();
        }
    }
}
