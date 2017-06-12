using System;

namespace FlatFinder.Model
{
    public class Offer : EntityBase
    {
        public int UserId { get; set; }

        public int FlatId { get; set; }
        public virtual Flat Flat { get; set; }

        public DateTime CreatedOn { get; set; }
        public OfferStatus Status { get; set; }
    }
}