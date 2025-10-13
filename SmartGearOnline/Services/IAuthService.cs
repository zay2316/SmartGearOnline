namespace SmartGearOnline.Services
{
    public interface IAuthService
    {
        bool Authenticate(string username, string password);
    }
}
