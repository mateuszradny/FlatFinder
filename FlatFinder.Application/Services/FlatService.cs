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
    public class FlatService : IFlatService
    {
        private readonly FlatFinderContext _context;
        private readonly IUserService _userService;

        public FlatService(FlatFinderContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Flat> Add(Flat flat)
        {
            if (flat == null)
                throw new ArgumentNullException(nameof(flat));

            flat.CreatedOn = flat.ModifiedOn = DateTime.UtcNow;
            flat.OwnerId = (await _userService.CurrentUser).Id;

            _context.Flats.Add(flat);
            await _context.SaveChangesAsync();

            return flat;
        }

        public async Task<SoldFlatsReport> GenerateReport(DateTime from, DateTime to)
        {
            DateTime fromDate = from.Date;
            DateTime toDate = to.Date.AddDays(1);

            var soldFlats = await _context.Flats.Where(x => x.IsSold && x.SoldOn >= fromDate && x.SoldOn < toDate).ToListAsync();

            return new SoldFlatsReport
            {
                From = from,
                To = to,
                Count = soldFlats.Count(),
                AverageArea = soldFlats.Any() ? soldFlats.Average(x => x.Area) : 0,
                AveragePrice = soldFlats.Any() ? soldFlats.Average(x => x.Price) : 0
            };
        }

        public async Task<Flat> Get(int id)
        {
            return await GetFlat(id, includeImages: true);
        }

        public async Task<IEnumerable<Flat>> Get()
        {
            var user = await _userService.CurrentUser;
            if (user == null)
                return await _context.Flats.Where(x => !x.IsSold).ToListAsync();

            return await _context.Flats.Where(x => !x.IsSold || x.OwnerId == user.Id).ToListAsync();
        }

        public async Task Remove(int id)
        {
            Flat flat = await GetFlatIfCurrentUserHasPermissions(id);

            _context.Flats.Remove(flat);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Flat>> Search(FlatSearchQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            IQueryable<Flat> flats = null;

            var user = await _userService.CurrentUser;
            if (user == null)
                flats = _context.Flats.Where(x => !x.IsSold);
            else
                flats = _context.Flats.Where(x => !x.IsSold || x.OwnerId == user.Id);

            if (query.MinNumberOfRooms.HasValue)
                flats = flats.Where(x => x.NumberOfRooms >= query.MinNumberOfRooms.Value);

            if (query.MaxNumberOfRooms.HasValue)
                flats = flats.Where(x => x.NumberOfRooms <= query.MaxNumberOfRooms.Value);

            if (query.MinArea.HasValue)
                flats = flats.Where(x => x.Area >= query.MinArea.Value);

            if (query.MaxArea.HasValue)
                flats = flats.Where(x => x.Area <= query.MaxArea.Value);

            if (query.MinPrice.HasValue)
                flats = flats.Where(x => x.Price >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                flats = flats.Where(x => x.Price <= query.MaxPrice.Value);

            if (query.HasBalcony.HasValue && query.HasBalcony.Value)
                flats = flats.Where(x => x.HasBalcony);

            return await flats.ToListAsync();
        }

        public async Task<Flat> SetAsSold(int id)
        {
            var flat = await GetFlatIfCurrentUserHasPermissions(id);
            if (flat.IsSold)
                throw new InvalidOperationException("The flat has already been set as sold.");

            flat.IsSold = true;
            flat.SoldOn = DateTime.Now.Date;
            await _context.SaveChangesAsync();

            return flat;
        }

        public async Task Update(Flat flat)
        {
            if (flat == null)
                throw new ArgumentNullException(nameof(flat));

            Flat existingFlat = await GetFlatIfCurrentUserHasPermissions(flat.Id);

            CopyUpdatabelProperties(from: flat, to: existingFlat);
            existingFlat.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private static void CopyUpdatabelProperties(Flat from, Flat to)
        {
            to.Address = from.Address;
            to.Area = from.Area;
            to.City = from.City;
            to.Description = from.Description;
            to.Floor = from.Floor;
            to.HasBalcony = from.HasBalcony;
            to.Name = from.Name;
            to.NumberOfRooms = from.NumberOfRooms;
            to.PostCode = from.PostCode;
            to.Price = from.Price;
        }

        private async Task<Flat> GetFlat(int id, bool includeImages = false)
        {
            IQueryable<Flat> query = _context.Flats.AsQueryable();
            if (includeImages)
                query.Include(x => x.Images);

            return await query.SingleOrDefaultAsync(x => x.Id == id)
                ?? throw new InvalidOperationException($"Flat with id {id} not exists.");
        }

        private async Task<Flat> GetFlatIfCurrentUserHasPermissions(int id)
        {
            var flat = await GetFlat(id);
            await ThrowExceptionIfUserDontHavePermissions(flat);

            return flat;
        }

        private async Task ThrowExceptionIfUserDontHavePermissions(Flat flat)
        {
            var user = await _userService.CurrentUser;
            if (user == null || user.Id != flat.OwnerId)
                throw new InvalidOperationException("You do not have permissions for this operation.");
        }
    }
}