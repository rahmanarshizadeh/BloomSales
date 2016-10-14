using BloomSales.Services.Contracts;
using BloomSales.Web.Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomSales.Web.Store.Controllers.Business
{
    public class CategoriesRetriever
    {
        private IInventoryService service;

        public CategoriesRetriever(IInventoryService inventoryService)
        {
            service = inventoryService;
        }

        public CategoryListViewModel GetList()
        {
            var allCategories = service.GetCategories();
            var categoriesList = new Dictionary<string, IList<string>>();

            // put the children under each parent and create the full object tree
            foreach (var cat in allCategories)
            {
                if (cat.Parent != null)
                {
                    if (!categoriesList.ContainsKey(cat.Parent.Name))
                        categoriesList.Add(cat.Parent.Name, new List<string>());

                    categoriesList[cat.Parent.Name].Add(cat.Name);
                }
            }

            // sort the children
            foreach (var key in categoriesList.Keys)
            {
                var children = categoriesList[key];
                ((List<string>)children).Sort();
            }

            return new CategoryListViewModel(categoriesList);
        }
    }
}