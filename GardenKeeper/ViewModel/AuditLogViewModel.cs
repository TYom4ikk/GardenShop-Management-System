using GardenKeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenKeeper.ViewModel
{
    class AuditLogViewModel
    {
        public List<AuditLog> AuditLogs { get; set; }
        public AuditLogViewModel()
        {
            AuditLogs = Core.context.AuditLog.ToList();
        }
    }
}
