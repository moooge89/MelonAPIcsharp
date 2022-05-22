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

            category.productCount = GetCountOfProducts(Id);

            return category;
        }

        private int GetCountOfProducts(int CategoryId)
        {
            string query = $"select category_id from product;";

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

            List<int?> categoryIds = new List<int?>();

            foreach (DataRow row in dataTable.Rows)
            {
                categoryIds.Add(row.Field<int?>("category_id"));
            }

            return categoryIds.Where(id => id == CategoryId).Count();
        }

        List<Category> ICategoryRepository.LoadAllCategories()
        {
            string query = @$"select c.id as id, c.name as name, c.content as content,
                           (select count(*)::int from product where category_id = c.id) as product_count
                           from category c;";

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
                    productCount = row.Field<int?>("product_count"),
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

                if (category.icon != null)
                {
                    parameter.Value = category.icon;
                }
                else
                {
                    parameter.Value = DBNull.Value;
                }

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

        public Category UpdateCategory(int id, Category category)
        {
            string query = @$"update category set name = '{category.name}',
                           content = @image
                           where id = {id};";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);

                var parameter = command.CreateParameter();
                parameter.ParameterName = "image";
                if (category.icon != null)
                {
                    parameter.Value = category.icon;
                }
                else
                {
                    parameter.Value = DBNull.Value;
                }

                command.Parameters.Add(parameter);

                command.ExecuteReader();

                con.Close();
            }

            return category;
        }

        public void DeleteCategoryById(int Id)
        {
            string query = @$"update product set category_id = null where category_id = {Id};
                           delete from category where id = {Id};";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using NpgsqlConnection con = new(sqlDataSource);
            con.Open();

            using NpgsqlCommand command = new(query, con);

            command.ExecuteNonQuery();

            con.Close();

        }
    }
}
