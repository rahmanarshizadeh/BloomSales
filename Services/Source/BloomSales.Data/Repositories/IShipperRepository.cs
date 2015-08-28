using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IShipperRepository : IRepository
    {
        void AddShipper(Shipper shipper);

        IEnumerable<Shipper> GetAllShippers();

        Shipper GetShipper(string name);
    }
}
