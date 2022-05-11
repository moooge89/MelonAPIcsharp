using MelonAPI.Model.exception;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class ContextRepository : IContextRepository
    {

        private readonly IConfiguration configuration;

        public ContextRepository (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int LoadCurrentUserId(string token)
        {
            if (token == null)
            {
                throw new RestException($"Authorize into system");
            }

            string query = $"select user_id as id from auth where token = '{token}'";

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
                throw new RestException($"Authorize again, your session has expired");
            }

            return dataTable.Rows[0].Field<int>("id");
        }
    }
}
