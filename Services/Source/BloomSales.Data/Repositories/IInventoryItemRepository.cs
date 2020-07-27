using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IInventoryItemRepository : IRepository
    {
        void AddToInventory(InventoryItem item);

        IEnumerable<InventoryItem> GetInventories(IEnumerable<Warehouse> warehouses);

        IEnumerable<InventoryItem> GetInventory(Warehouse warehouse);

        InventoryItem GetStock(Warehouse warehouse, int productID);

        IEnumerable<InventoryItem> GetStocks(IEnumerable<Warehouse> warehouses, int productID);

        void UpdateStock(int inventoryItemID, short newStock);
    }
}