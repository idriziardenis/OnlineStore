using OnlineStore.Areas.Store.Models;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Services
{
    public class SelectListService : ISelectListService
    {
        public IEnumerable<KeyValueItem> GetGendersKeysValues()
        {
            try
            {
                return new List<KeyValueItem>()
                {
                    new KeyValueItem(){
                        BKey = true,
                        Value = "Man"
                    },
                    new KeyValueItem(){
                        BKey = false,
                        Value = "Woman"
                    },
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
