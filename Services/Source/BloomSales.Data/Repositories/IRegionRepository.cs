using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public interface IRegionRepository : IRepository
    {
        IEnumerable<Region> GetAllRegions();

        IEnumerable<Region> GetAllRegionsByCountry(string country);

        Region GetRegion(int id);

        Region GetRegion(string name);

        void AddRegion(Region region);
    }
}
