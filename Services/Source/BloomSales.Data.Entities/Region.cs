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
    public class Region
    {
        [DataMember]
        public int ID { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [Required]
        [DataMember]
        public string Continent { get; set; }

        [Required]
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public virtual IEnumerable<Province> Provinces { get; set; }
    }
}