using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlatFinder.Contracts.Services
{
    public interface IUserService
    {
        Task<User> CurrentUser { get; }

        Task ChangePassword(int userId, string newPassword);

        Task<IEnumerable<User>> GetAll();

        Task<User> GetUser(string email);

        Task<User> Register(string email, string password, string phoneNumber);

        Task Remove(int userId);

        Task<bool> VerifyCredentials(string email, string password);
    }
}