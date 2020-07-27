using BloomSales.Data.Entities;
using System.Collections.Generic;

namespace BloomSales.Data.Repositories
{
    public interface IRegionRepository : IRepository
    {
        void AddRegion(Region region);

        IEnumerable<Region> GetAllRegions();

        IEnumerable<Region> GetAllRegionsByCountry(string country);

        Region GetRegion(int id);

        Region GetRegion(string name);

        Region GetRegionByProvince(string country, string province);
    }
}