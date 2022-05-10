using MelonAPI.Model;

namespace MelonAPI.Repository
{
    public interface IUserRepository
    {
        User FindUserByEmailAndPassword(LoginInfo loginInfo);

        int CreateUser(LoginInfo loginInfo);
    }
}
