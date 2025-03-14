using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.ViewModel
{
    public class ProductCustomizationViewModel
    {
        public List<Model.Properties> properties;
        public ProductCustomizationViewModel()
        {
            properties = Core.context.Properties.ToList();
        }
        public void AddNewProperty(string name, string value)
        {
            Model.Properties property = new Model.Properties { Name = name, Value = value};
        }
    }
}
