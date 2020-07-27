using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Product : IIdentifiable
    {
        [DataMember]
        public virtual ProductCategory Category { get; set; }

        [Required]
        [DataMember]
        public int CategoryID { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        [DataMember]
        public bool IsDiscontinued { get; set; }

        [Required]
        [StringLength(120)]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Producer { get; set; }

        [Required]
        [DataMember]
        public decimal UnitPrice { get; set; }
    }
}