using FlatFinder.Model;
using System.Threading.Tasks;

namespace FlatFinder.Contracts.Services
{
    public interface IImageService
    {
        Task<Image> Add(Image image);

        Task Remove(int id);
    }
}