using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IUserRepository
    {
        User LoadUserByEmailAndPassword(LoginInfo loginInfo);

        int CreateUser(LoginInfo loginInfo);
    }
}
