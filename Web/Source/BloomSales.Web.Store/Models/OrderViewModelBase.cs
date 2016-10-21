using BloomSales.Data.Entities;

namespace BloomSales.Web.Store.Models
{
    public class OrderViewModelBase
    {
        protected static string GetStatusTitle(ShippingStatus status)
        {
            switch (status)
            {
                case ShippingStatus.OutForDelivery:
                    return "Out for Delivery";

                case ShippingStatus.PickedUp:
                    return "Picked up";

                case ShippingStatus.ReceivedOrder:
                    return "Received order";

                default:
                    return status.ToString();
            }
        }
    }
}