using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private LocationDb db;

        public ProvinceRepository()
        {
            this.db = new LocationDb();
        }

        internal ProvinceRepository(LocationDb context)
        {
            db = context;
        }

        public IEnumerable<Province> GetProvincesForRegion(int regionID)
        {
            var result = (from p in db.Provinces
                          where p.RegionID == regionID
                          select p).ToArray();

            return result;
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}