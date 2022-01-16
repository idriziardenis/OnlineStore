using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.StoreManagement.Models
{
    public class DashboardViewModel
    {
        public int NoProducts { get; set; }
        public int NoCategories { get; set; }
        public int NoOrders { get; set; }
        public int NoClients { get; set; }
    }
}
