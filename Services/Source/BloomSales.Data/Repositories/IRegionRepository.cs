using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IRegionRepository : IRepository
    {
        IEnumerable<Region> GetAllRegions();

        IEnumerable<Region> GetAllRegionsByCountry(string country);

        Region GetRegion(int id);

        Region GetRegion(string name);

        Region GetRegionByProvince(string country, string province);

        void AddRegion(Region region);
    }
}