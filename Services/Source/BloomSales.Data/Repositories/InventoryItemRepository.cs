using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BloomSales.Data.Repositories
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private InventoryDb db;

        public InventoryItemRepository()
        {
            this.db = new InventoryDb();
        }

        internal InventoryItemRepository(InventoryDb context)
        {
            this.db = context;
        }

        public void AddToInventory(InventoryItem item)
        {
            this.db.Inventories.Add(item);
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            if (this.db != null)
                db.Dispose();
        }

        public IEnumerable<InventoryItem> GetInventories(IEnumerable<Warehouse> warehouses)
        {
            List<InventoryItem> result = new List<InventoryItem>();

            foreach (Warehouse w in warehouses)
            {
                var list = GetInventory(w);
                result.AddRange(list);
            }

            return result;
        }

        public IEnumerable<InventoryItem> GetInventory(Warehouse warehouse)
        {
            var result = (from i in db.Inventories
                          where i.WarehouseID == warehouse.ID
                          select i).ToArray();

            return result;
        }

        public InventoryItem GetStock(Warehouse warehouse, int productID)
        {
            var result = (from i in db.Inventories
                          where (i.WarehouseID == warehouse.ID) && (i.ProductID == productID)
                          select i).Single();

            return result;
        }

        public IEnumerable<InventoryItem> GetStocks(IEnumerable<Warehouse> warehouses, int productID)
        {
            List<InventoryItem> result = new List<InventoryItem>();

            foreach (Warehouse w in warehouses)
            {
                var stock = GetStock(w, productID);
                result.Add(stock);
            }

            return result;
        }

        public void UpdateStock(int inventoryItemID, short newStock)
        {
            var item = (from i in db.Inventories
                        where i.ID == inventoryItemID
                        select i).Single();

            if (item.UnitsInStock != newStock)
            {
                item.UnitsInStock = newStock;
                db.SaveChanges();
            }
        }
    }
}