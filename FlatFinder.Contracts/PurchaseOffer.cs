using FlatFinder.Model;
using System;

namespace FlatFinder.Contracts
{
    public class PurchaseOffer
    {
        public int OfferId { get; set; }

        public string UserEmail { get; set; }
        public string UserPhone { get; set; }

        public int FlatId { get; set; }
        public string FlatName { get; set; }

        public DateTime CreatedOn { get; set; }
        public OfferStatus Status { get; set; }
    }
}