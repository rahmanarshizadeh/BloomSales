using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BloomSales.Data.Entities
{
    [DataContract]
    public class Region
    {
        [Required]
        [DataMember]
        public string Continent { get; set; }

        [Required]
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual IEnumerable<Province> Provinces { get; set; }
    }
}