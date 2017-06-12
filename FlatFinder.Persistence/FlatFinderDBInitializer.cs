using FlatFinder.Contracts.Services;
using FlatFinder.Model;
using System.Data.Entity;

namespace FlatFinder.Persistence
{
    public class FlatFinderDBInitializer : DropCreateDatabaseIfModelChanges<FlatFinderContext>
    {
        private readonly ICryptographyService _cryptographyService;

        public FlatFinderDBInitializer(ICryptographyService cryptographyService)
        {
            _cryptographyService = cryptographyService;
        }

        protected override void Seed(FlatFinderContext context)
        {
            Role adminRole = new Role() { Name = "Admin" };
            context.Roles.Add(adminRole);
            context.SaveChanges();

            string password = "P@ssw0rd";
            byte[] salt = _cryptographyService.GetSalt();

            User admin = new User()
            {
                Email = "admin@flatfinder.com",
                HashedPassword = _cryptographyService.HashPassword(password, salt),
                Salt = salt,
                Roles = { adminRole }
            };

            context.Users.Add(admin);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}