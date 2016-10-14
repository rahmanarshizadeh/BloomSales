using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomSales.Web.Store.Models
{
    public class CategoryListViewModel
    {
        private IDictionary<string, IList<string>> list;

        public CategoryListViewModel(IDictionary<string, IList<string>> categoriesList)
        {
            list = categoriesList;
        }

        public IEnumerable<string> Parents
        {
            get { return list.Keys; }
        }

        public IEnumerable<string> GetChildren(string parentName)
        {
            return list[parentName];
        }
    }
}