using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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