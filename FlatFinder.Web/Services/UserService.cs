using FlatFinder.Contracts;
using FlatFinder.Contracts.Services;
using FlatFinder.Persistence;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlatFinder.Web.Services
{
    public class UserService : IUserService
    {
        private readonly FlatFinderContext _context;
        private readonly ICryptographyService _cryptographyService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(FlatFinderContext context, ICryptographyService cryptographyService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cryptographyService = cryptographyService;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<User> CurrentUser => GetCurrentUser();

        public async Task ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new InvalidOperationException($"User with id {userId} not exists.");

            user.HashedPassword = _cryptographyService.HashPassword(newPassword, user.Salt);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(x => x.Roles).Select(user => new User
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.Email,
                Roles = user.Roles.Select(x => x.Name)
            }).ToListAsync();
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
                throw new InvalidOperationException($"User with email address {email} not exists.");

            return new User
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles.Select(x => x.Name)
            };
        }

        public async Task<User> Register(string email, string password, string phoneNumber)
        {
            if (!IsValidEmail(email))
                throw new InvalidOperationException("Invalid address email.");

            if (_context.Users.Any(x => x.Email == email))
                throw new InvalidOperationException($"User with email address {email} already exists.");

            byte[] salt = _cryptographyService.GetSalt();
            string hashedPassword = _cryptographyService.HashPassword(password, salt);

            var user = _context.Users.Create();
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.HashedPassword = hashedPassword;
            user.Salt = salt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new User
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = user.Roles.Select(x => x.Name)
            };
        }

        public async Task Remove(int userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new InvalidOperationException($"User with id {userId} not exists.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerifyCredentials(string email, string password)
        {
            if (!IsValidEmail(email))
                throw new InvalidOperationException("Invalid address email.");

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
                throw new InvalidOperationException($"User with email address {email} not exists.");

            string hashedPassword = _cryptographyService.HashPassword(password, user.Salt);
            return hashedPassword == user.HashedPassword;
        }

        private async Task<User> GetCurrentUser()
        {
            ClaimsPrincipal principal = _httpContextAccessor.HttpContext.User;

            string email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await GetUser(email);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}