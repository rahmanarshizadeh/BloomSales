using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Shipper : ContactInfo
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public virtual IEnumerable<DeliveryService> Services { get; set; }
    }
}