using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IProvinceRepository : IRepository
    {
        IEnumerable<Province> GetProvincesForRegion(int regionID);
    }
}