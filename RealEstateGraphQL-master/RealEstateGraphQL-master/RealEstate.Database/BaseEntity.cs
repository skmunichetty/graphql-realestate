using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Database
{
    public abstract class BaseEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
