using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.Model
{
    /// <summary>
    /// Статический класс, предоставляющий доступ к контексту базы данных
    /// </summary>
    public static class Core
    {
        /// <summary>
        /// Статический экземпляр контекста базы данных GardenStoreEntities
        /// </summary>
        public static GardenStoreEntities context = new GardenStoreEntities();
    }
}
