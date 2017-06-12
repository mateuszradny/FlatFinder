using FlatFinder.Contracts;
using FlatFinder.Contracts.Services;
using FlatFinder.Model;
using FlatFinder.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FlatFinder.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly FlatFinderContext _context;
        private readonly IFlatService _flatService;
        private readonly IUserService _userService;

        public OfferService(FlatFinderContext context, IFlatService flatService, IUserService userService)
        {
            _context = context;
            _flatService = flatService;
            _userService = userService;
        }

        public async Task<Offer> Add(int flatId)
        {
            var flat = await _flatService.Get(flatId);
            var user = await _userService.CurrentUser;

            if (flat.OwnerId == user.Id)
                throw new InvalidOperationException("You can not add purchase offers for flats which have added by you.");

            var offer = await GetOffer(flatId, user.Id);
            if (offer != null)
                throw new InvalidOperationException("You have added a purchase offer for this flat.");

            offer = new Offer { FlatId = flatId, UserId = user.Id, Status = OfferStatus.New, CreatedOn = DateTime.Now };

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();

            return offer;
        }

        public async Task<IEnumerable<PurchaseOffer>> Get()
        {
            var user = await _userService.CurrentUser;
            return await _context.Offers.Where(x => x.Status != OfferStatus.Removed && x.Flat.OwnerId == user.Id).Join(_context.Users, x => x.UserId, x => x.Id, (o, u) => new PurchaseOffer
            {
                OfferId = o.Id,
                FlatId = o.FlatId,
                FlatName = o.Flat.Name,
                UserEmail = u.Email,
                UserPhone = u.PhoneNumber,
                CreatedOn = o.CreatedOn,
                Status = o.Status
            }).OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public async Task<int> GetNewOfferCount()
        {
            var user = await _userService.CurrentUser;

            return await _context.Offers.Where(x => x.Status == OfferStatus.New && x.Flat.OwnerId == user.Id).CountAsync();
        }

        public async Task Remove(int offerId)
        {
            var offer = await GetOffer(offerId);
            var user = await _userService.CurrentUser;

            var flat = await _flatService.Get(offer.FlatId);
            if (flat.OwnerId != user.Id)
                throw new InvalidOperationException("You do not have permissions for this operation.");

            offer.Status = OfferStatus.Removed;
            await _context.SaveChangesAsync();
        }

        public async Task SetAsRead(int[] offerIds)
        {
            var user = await _userService.CurrentUser;
            var offers = await _context.Offers.Where(x => x.Flat.OwnerId == user.Id && offerIds.Contains(x.Id) && x.Status == OfferStatus.New).ToListAsync();

            foreach (var offer in offers)
                offer.Status = OfferStatus.Active;

            await _context.SaveChangesAsync();
        }

        private async Task<Offer> GetOffer(int flatId, int userId)
        {
            return await _context.Offers.SingleOrDefaultAsync(x => x.FlatId == flatId && x.UserId == userId);
        }

        private async Task<Offer> GetOffer(int offerId)
        {
            return await _context.Offers.SingleOrDefaultAsync(x => x.Id == offerId)
                ?? throw new InvalidOperationException($"Offer with id {offerId} not exists.");
        }
    }
}