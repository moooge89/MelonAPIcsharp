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
            string query = $"select * from category;";

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

            List<Category> categories = new List<Category>();

            foreach (DataRow row in dataTable.Rows)
            {
                categories.Add(new Category()
                {
                    id = row.Field<int>("id"),
                    name = row.Field<string>("name"),
                    icon = row.Field<byte[]?>("content"),
                });
            }

            Category category = categories.Where(category => category.id == Id).FirstOrDefault(new Category());

            if (category == null)
            {
                throw new IdNotFoundException($"Category with ID {Id} not found");
            }

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
                    id = row.Field<int>("id"),
                    name = row.Field<string>("name"),
                    icon = row.Field <byte[]?>("content"),
                };
                categories.Add(category);
            }

            return categories;
        }

        Category ICategoryRepository.SaveCategory(Category category)
        {
            string query = @$"insert into category (name, content) values
                           ('{category.name}', @image)
                           returning id;";

            DataTable dataTable = new();
            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");
            NpgsqlDataReader dataReader;

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);

                var parameter = command.CreateParameter();
                parameter.ParameterName = "image";
                parameter.Value = category.icon;

                command.Parameters.Add(parameter);


                dataReader = command.ExecuteReader();
                dataTable.Load(dataReader);

                dataReader.Close();
                con.Close();
            }

            DataRow dataRow = dataTable.Rows[0];

            category.id = dataRow.Field<int>("id");

            return category;
        }
    }
}
