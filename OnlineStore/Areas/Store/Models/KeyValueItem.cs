using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Store.Models
{
    public class KeyValueItem
    {
        public int Key { get; set; }
        public string SKey { get; set; }
        public bool BKey { get; set; }
        public string Value { get; set; }
    }
}
