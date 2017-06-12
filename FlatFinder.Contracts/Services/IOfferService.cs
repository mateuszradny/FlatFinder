using FlatFinder.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlatFinder.Contracts.Services
{
    public interface IOfferService
    {
        Task<Offer> Add(int flatId);

        Task<IEnumerable<PurchaseOffer>> Get();

        Task<int> GetNewOfferCount();

        Task Remove(int offerId);

        Task SetAsRead(int[] offerIds);
    }
}