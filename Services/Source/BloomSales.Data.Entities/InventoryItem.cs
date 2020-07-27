using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class InventoryItem
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public virtual Product Product { get; set; }

        [Required]
        [DataMember]
        public int ProductID { get; set; }

        [Required]
        [DataMember]
        public short UnitsInStock { get; set; }

        [NotMapped]
        [DataMember]
        public Warehouse Warehouse { get; set; }

        [Required]
        [DataMember]
        public int WarehouseID { get; set; }
    }
}