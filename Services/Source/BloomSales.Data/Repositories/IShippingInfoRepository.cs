using BloomSales.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Repositories
{
    public interface IShippingInfoRepository : IRepository
    {
        void AddShipping(ShippingInfo shipping);

        ShippingStatus GetShippingStatus(int orderID);
    }
}
