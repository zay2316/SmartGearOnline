namespace SmartGearOnline.Services
{
    public class AuthService : IAuthService
    {
        public bool Authenticate(string username, string password)
        {
            //Auth Logic for testing
            return username == "admin" && password == "password";
        }
    }
}
