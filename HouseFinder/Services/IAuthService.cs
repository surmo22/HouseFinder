using HouseFinderBackEnd.Data;

namespace HouseFinderBackEnd.Services
{
    public interface IAuthService
    {
        public Task<string> Register(RegisterModel model);
        public Task<string> Login(LoginModel model);
    }
}
