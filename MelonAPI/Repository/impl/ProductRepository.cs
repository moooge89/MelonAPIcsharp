using MelonAPI.Model;
using MelonAPI.Model.exception;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class ProductRepository : IProductRepository
    {

        private readonly IConfiguration configuration;

        public ProductRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public List<ProductLight> LoadProductByCategoryId(int CategoryId, int userId)
        {
            string query = $"select * from product where category_id = {CategoryId}";

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

            List<ProductLight> products = new();

            foreach (DataRow row in dataTable.Rows)
            {
                ProductLight product = new()
                {
                    Id = row.Field<int>("id"),
                    Name = row.Field<string>("name"),
                    Price = row.Field<decimal>("price"),
                    CategoryId = row.Field<int>("category_id"),
                    IsInCart = false,
                    IsInWishlist = false,
                };
                products.Add(product);
            }

            return products;
        }

        public Product LoadProductById(int productId, int userId)
        {
            string query = @$"select p.id as product_id, p.name as product_name,
                            p.description, p.price, p.count, p.manufacturer,
                            p.category_id, c.id as category_id, c.name as category_name
                            from product p, category c where p.category_id = c.id and p.id = {productId};";

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
                throw new IdNotFoundException($"Product with id {productId} was not found");
            }

            Product product = new()
            {
                Id = dataTable.Rows[0].Field<int>("product_id"),
                Name = dataTable.Rows[0].Field<string>("product_name"),
                Description = dataTable.Rows[0].Field<string>("description"),
                Price = dataTable.Rows[0].Field<decimal>("price"),
                Count = dataTable.Rows[0].Field<int>("count"),
                Manufacturer = dataTable.Rows[0].Field<string>("manufacturer"),
            };

            Category category = new()
            {
                Id = dataTable.Rows[0].Field<int>("category_id"),
                Name = dataTable.Rows[0].Field<string>("category_name"),
            };

            product.Category = category;

            return product;
        }
    }
}
