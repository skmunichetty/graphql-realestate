using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Database.Models
{
    public class Property : BaseEntity
    {
        //public string Id => Url.ToString()?.Split('/')[5];
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public Uri Url { get; set; }
        //public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }
        public string Family { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
