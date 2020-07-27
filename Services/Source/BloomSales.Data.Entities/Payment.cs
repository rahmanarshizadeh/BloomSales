using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class PaymentInfo
    {
        [Required]
        [DataMember]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3)]
        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public bool IsReceived { get; set; }

        [Required]
        [DataMember]
        public int OrderID { get; set; }

        [DataMember]
        public DateTime ReceivedDate { get; set; }

        [Required]
        [DataMember]
        public PaymentType Type { get; set; }
    }
}