using FlatFinder.Contracts.Services;
using FlatFinder.Model;
using System.Data.Entity;

namespace FlatFinder.Persistence
{
    public class FlatFinderContext : DbContext
    {
        public FlatFinderContext(string nameOrConnectionString, ICryptographyService cryptographyService) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new FlatFinderDBInitializer(cryptographyService));
        }

        public DbSet<Image> Images { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
    }
}