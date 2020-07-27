using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IShipperRepository : IRepository
    {
        void AddShipper(Shipper shipper);

        IEnumerable<Shipper> GetAllShippers();

        Shipper GetShipper(string name);
    }
}