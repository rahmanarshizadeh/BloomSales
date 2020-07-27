using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class ProductCategory
    {
        [DataMember]
        public string Description { get; set; }

        [Key]
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual ProductCategory Parent { get; set; }

        [DataMember]
        public int? ParentID { get; set; }
    }
}