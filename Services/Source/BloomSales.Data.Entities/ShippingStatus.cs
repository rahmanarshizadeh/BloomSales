
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    public enum ShippingStatus
    {
        None,
        ReceivedOrder,
        PickedUp,
        Packaging,
        InTransit,
        OutForDelivery,
        Delivered
    }
}