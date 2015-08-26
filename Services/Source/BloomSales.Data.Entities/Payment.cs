using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    public class PaymentInfo
    {
        public int ID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public PaymentType Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public DateTime ReceivedDate { get; set; }

        public bool IsReceived { get; set; }
    }
}
