using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Services.Tests
{
    public static class TestHelpers
    {
        public static List<T> CreateList<T>(int itemsCount) where T : IIdentifiable, new()
        {
            var list = new List<T>();

            for (int i = 1; i <= itemsCount; i++)
                list.Add(new T() { ID = i });

            return list;
        }
    }
}