using BloomSales.Data.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BloomSales.Data.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private LocationDb db;
        private IRegionRepository regionRepo;

        public WarehouseRepository()
        {
            this.db = new LocationDb();
            this.regionRepo = new RegionRepository(this.db);
        }

        internal WarehouseRepository(LocationDb context, IRegionRepository repo)
        {
            this.db = context;
            this.regionRepo = repo;
        }

        public void AddWarehouse(Warehouse warehouse)
        {
            Debug.Assert(warehouse != null, "The given warehouse argument should not be null");

            db.Warehouses.Add(warehouse);
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();

            if (regionRepo != null)
                regionRepo.Dispose();
        }

        public Warehouse GetWarehouse(string name)
        {
            var result = (from w in db.Warehouses
                          where name.Equals(w.Name)
                          select w).Single();

            return result;
        }

        public Warehouse GetWarehouse(int id)
        {
            return db.Warehouses.Find(id);
        }

        public IEnumerable<Warehouse> GetWarehousesByCity(string city)
        {
            var result = (from w in db.Warehouses
                          where city.Equals(w.City)
                          select w).ToArray();

            return result;
        }

        public IEnumerable<Warehouse> GetWarehousesByProvince(string province)
        {
            var result = (from w in db.Warehouses
                          where province.Equals(w.Province)
                          select w).ToArray();

            return result;
        }

        public IEnumerable<Warehouse> GetWarehousesByRegion(string region)
        {
            Region r = this.regionRepo.GetRegion(region);

            var result = (from w in db.Warehouses
                          where w.RegionID == r.ID
                          select w).ToArray();

            return result;
        }

        public void RemoveWarehouse(Warehouse warehouse)
        {
            db.Warehouses.Attach(warehouse);
            db.Warehouses.Remove(warehouse);
            db.SaveChanges();
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            Warehouse oldW = db.Warehouses.Find(warehouse.ID);
            UpdateEntity(warehouse, oldW);
            db.SaveChanges();
        }

        private static void UpdateEntity(Warehouse updatedEntity, Warehouse oldEntity)
        {
            oldEntity.City = updatedEntity.City;
            oldEntity.Country = updatedEntity.Country;
            oldEntity.Email = updatedEntity.Email;
            oldEntity.Name = updatedEntity.Name;
            oldEntity.Phone = updatedEntity.Phone;
            oldEntity.PostalCode = updatedEntity.PostalCode;
            oldEntity.Province = updatedEntity.Province;
            oldEntity.Region = updatedEntity.Region;
            oldEntity.RegionID = updatedEntity.RegionID;
            oldEntity.StreetAddress = updatedEntity.StreetAddress;
        }
    }
}