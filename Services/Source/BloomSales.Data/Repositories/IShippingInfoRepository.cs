using BloomSales.Data.Entities;

namespace BloomSales.Data.Repositories
{
    public interface IShippingInfoRepository : IRepository
    {
        void AddShipping(ShippingInfo shipping);

        ShippingInfo GetShipping(int orderID);

        ShippingStatus GetShippingStatus(int orderID);
    }
}