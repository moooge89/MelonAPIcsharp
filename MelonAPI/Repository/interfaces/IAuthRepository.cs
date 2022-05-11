using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IAuthRepository
    {
        string Login(LoginInfo loginInfo);

        void Logout(int userId);

        string Register(LoginInfo loginInfo);
    }
}
