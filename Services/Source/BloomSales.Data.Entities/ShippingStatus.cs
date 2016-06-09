
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
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