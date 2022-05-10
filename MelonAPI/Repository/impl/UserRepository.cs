using MelonAPI.Model;
using MelonAPI.Model.exception;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class UserRepository : IUserRepository
    {

        private readonly IConfiguration configuration;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int CreateUser(LoginInfo loginInfo)
        {
            string query = $"insert into user_ (email, password) values ({loginInfo.email}, {loginInfo.email});";
            
            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);
                command.ExecuteReader();
                con.Close();
            }

            return FindUserByEmailAndPassword(loginInfo).id;
        }

        public User FindUserByEmailAndPassword(LoginInfo loginInfo)
        {
            string query = $"select * from user_ where email = {loginInfo.email} and password = {loginInfo.password};";

            DataTable dataTable = new();
            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);
                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);

                dataReader.Close();
                con.Close();
            }

            if (dataTable.Rows.Count != 1)
            {
                throw new RestException($"User with email {loginInfo.email} already exists");
            }

            User user = new()
            {
                id = dataTable.Rows[0].Field<int>("id"),
                email = dataTable.Rows[0].Field<string>("email"),
                password = dataTable.Rows[0].Field<string>("password")
            };

            return user;
        }
    }
}
