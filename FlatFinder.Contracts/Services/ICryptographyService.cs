namespace FlatFinder.Contracts.Services
{
    public interface ICryptographyService
    {
        byte[] GetSalt();

        string HashPassword(string password, byte[] salt);
    }
}