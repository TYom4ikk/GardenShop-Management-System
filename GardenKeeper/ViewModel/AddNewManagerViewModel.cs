using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GardenKeeper.Model;

namespace GardenKeeper.ViewModel
{
    internal class AddNewManagerViewModel
    {
        /// <summary>
        /// Добавление менеджера в базу данных
        /// </summary>
        /// <param name="user"></param>
        public void AddNewManager(Model.Users user)
        {
            Core.context.Users.Add(user);
            Core.context.SaveChanges();
        }
    }
}
