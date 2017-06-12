using System;
using System.Collections.Generic;

namespace FlatFinder.Model
{
    public class Flat : EntityBase
    {
        public string Address { get; set; }
        public double Area { get; set; }
        public string City { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }
        public int Floor { get; set; }
        public bool HasBalcony { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Name { get; set; }
        public int NumberOfRooms { get; set; }
        public string PostCode { get; set; }
        public double Price { get; set; }
        public bool IsSold { get; set; }
        public DateTime? SoldOn { get; set; }

        public int OwnerId { get; set; }
        protected virtual User Owner { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}