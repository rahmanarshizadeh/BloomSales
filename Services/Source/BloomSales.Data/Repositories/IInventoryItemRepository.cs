using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IInventoryItemRepository : IRepository
    {
        IEnumerable<InventoryItem> GetInventory(Warehouse warehouse);

        IEnumerable<InventoryItem> GetInventories(IEnumerable<Warehouse> warehouses);

        InventoryItem GetStock(Warehouse warehouse, int productID);

        IEnumerable<InventoryItem> GetStocks(IEnumerable<Warehouse> warehouses, int productID);

        void AddToInventory(InventoryItem item);

        void UpdateStock(int inventoryItemID, short newStock);
    }
}
