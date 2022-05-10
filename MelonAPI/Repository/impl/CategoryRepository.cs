using MelonAPI.Model;
using MelonAPI.Model.exception;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly IConfiguration configuration;

        public CategoryRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        Category ICategoryRepository.LoadCategoryById(int Id)
        {

            string query = $"select * from category where id = {Id}";

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
                throw new IdNotFoundException($"Category with id {Id} was not found");
            }

            Category category = new()
            {
                Id = dataTable.Rows[0].Field<int>("id"),
                Name = dataTable.Rows[0].Field<string>("name")
            };

            return category;
        }

        List<Category> ICategoryRepository.LoadAllCategories()
        {
            string query = $"select * from category";

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

            List<Category> categories = new();

            foreach (DataRow row in dataTable.Rows)
            {
                Category category = new()
                {
                    Id = row.Field<int>("id"),
                    Name = row.Field<string>("name")
                };
                categories.Add(category);
            }

            return categories;
        }
    }
}
