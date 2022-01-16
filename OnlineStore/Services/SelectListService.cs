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
        private ICategoryRepository _categoryRepository;

        public SelectListService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<KeyValueItem> GetCategoriesKeysValues()
        {
            try
            {
                var categories = _categoryRepository.Find(p => p.IsActive == true && p.IsDeleted == false);

                var result = categories.Select(category => new KeyValueItem()
                {
                    Key = category.Id,
                    Value = category.Name
                });

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<KeyValueItem> GetGendersKeysValues(string man, string woman)
        {
            try
            {
                return new List<KeyValueItem>()
                {
                    new KeyValueItem(){
                        BKey = true,
                        Value = man
                    },
                    new KeyValueItem(){
                        BKey = false,
                        Value = woman
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
