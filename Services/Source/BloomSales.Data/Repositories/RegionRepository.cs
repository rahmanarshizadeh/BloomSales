using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private LocationDb db;

        public RegionRepository()
        {
            this.db = new LocationDb();
        }

        internal RegionRepository(LocationDb context)
        {
            this.db = context;
        }

        public IEnumerable<Region> GetAllRegions()
        {
            return db.Regions.ToArray();
        }

        public IEnumerable<Region> GetAllRegionsByCountry(string country)
        {
            var regions = (from region in db.Regions
                           where country.Equals(region.Country)
                           select region).ToArray();

            return regions;
        }

        public Region GetRegion(int id)
        {
            Region region = db.Regions.Find(id);

            return region;
        }

        public Region GetRegion(string name)
        {
            var region = (from r in db.Regions
                          where name.Equals(r.Name)
                          select r).Single();

            return region;
        }

        public Region GetRegionByProvince(string country, string province)
        {
            var regions = GetAllRegionsByCountry(country);

            foreach (Region r in regions)
            {
                r.Provinces = db.Provinces
                    .Where(p => p.RegionID == r.ID)
                    .ToArray();

                foreach (Province p in r.Provinces)
                    if (p.Name == province || p.Abbreviation == province)
                        return r;
            }

            return null;
        }

        public void AddRegion(Region region)
        {
            db.Regions.Add(region);
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}