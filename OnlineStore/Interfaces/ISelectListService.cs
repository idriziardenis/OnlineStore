using OnlineStore.Areas.Store.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Interfaces
{
    public interface ISelectListService
    {
        IEnumerable<KeyValueItem> GetGendersKeysValues(string man, string woman);
        IEnumerable<KeyValueItem> GetCategoriesKeysValues();
    }
}
