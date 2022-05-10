using MelonAPI.Model;
using MelonAPI.Model.exception;
using Npgsql;

namespace MelonAPI.Repository.impl
{
    public class AuthRepository : IAuthRepository
    {

        private readonly IConfiguration configuration;

        private readonly IUserRepository userRepository;

        public AuthRepository(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        public string Login(LoginInfo loginInfo)
        {
            if (loginInfo == null || loginInfo.email == null || loginInfo.password == null)
            {
                throw new RestException("Credientials cannot be null");
            }

            User user = userRepository.FindUserByEmailAndPassword(loginInfo);
            
            if (user == null)
            {
                throw new RestException("No user not found with given credintials");
            }

            ClearSessionByUserId(user.id);

            return SetSessionByUserId(user.id);
        }

        public void Logout(int userId)
        {
            ClearSessionByUserId(userId); 
        }

        public string Register(LoginInfo loginInfo)
        {
            int userId = userRepository.CreateUser(loginInfo);
            return SetSessionByUserId(userId);
        }

        private void ClearSessionByUserId(int userId)
        {
            string query = $"delete from auth where userId = {userId}";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using NpgsqlConnection con = new(sqlDataSource);
            con.Open();

            using NpgsqlCommand command = new(query, con);
            command.ExecuteReader();
            con.Close();
        }

        private string SetSessionByUserId(int userId)
        {

            string token = GenerateId();

            string query = $"insert into auth (token, userId) values ({token}, {userId});";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using NpgsqlConnection con = new(sqlDataSource);
            con.Open();

            using NpgsqlCommand command = new(query, con);
            command.ExecuteReader();
            con.Close();

            return token;
        }

        private static string GenerateId()
        {
            string letters = "abcdefghijklmnopqrstuvwxyz";

            string res = "";
            Random random = new Random();

            for (int i = 0; i < 16; ++i)
            {
                res += letters[random.Next(letters.Length - 1)];
            }

            return res;
        }
    }
}
