using FlatFinder.Contracts.Services;
using FlatFinder.Model;
using FlatFinder.Persistence;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FlatFinder.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly FlatFinderContext _context;
        private readonly IFlatService _flatService;
        private readonly IUserService _userService;

        public ImageService(FlatFinderContext context, IFlatService flatService, IUserService userService)
        {
            _context = context;
            _flatService = flatService;
            _userService = userService;
        }

        public async Task<Image> Add(Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            var flat = await _flatService.Get(image.FlatId);
            var user = await _userService.CurrentUser;

            if (flat.OwnerId != user.Id)
                throw new InvalidOperationException("You do not have permissions for this operation.");

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task Remove(int id)
        {
            var image = await GetImage(id, includeFlat: true);
            var user = await _userService.CurrentUser;

            if (image.Flat.OwnerId != user.Id)
                throw new InvalidOperationException("You do not have permissions for this operation.");

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        private async Task<Image> GetImage(int id, bool includeFlat = false)
        {
            IQueryable<Image> query = _context.Images.AsQueryable();

            if (includeFlat)
                query.Include(x => x.Flat);

            return await query.SingleOrDefaultAsync(x => x.Id == id)
                ?? throw new InvalidOperationException($"Image with id {id} not exists.");
        }
    }
}