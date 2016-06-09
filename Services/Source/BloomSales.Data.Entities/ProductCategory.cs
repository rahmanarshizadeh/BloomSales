using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class ProductCategory
    {
        [Key]
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int? ParentID { get; set; }

        [DataMember]
        public virtual ProductCategory Parent { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}