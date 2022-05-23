using MelonAPI.Model;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class WishlistRepository : IWishlistRepository
    {

        private readonly IConfiguration configuration;

        public WishlistRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void AddProductIntoWishlist(int productId, int userId)
        {
            string query = @$"insert into wishlist (product_id, user_id) values ({productId}, {userId})
                           on conflict (product_id, user_id) do nothing;";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);
                command.ExecuteReader();
                con.Close();
            }
        }

        public void DeleteProductFromWishlist(int productId, int userId)
        {
            string query = $"delete from wishlist where product_id = {productId} and user_id = {userId};";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);
                command.ExecuteReader();
                con.Close();
            }
        }

        public List<Product> LoadWishlistProducts(int userId)
        {
            string query = @$"select p.id as p_id, p.name as p_name, p.description as p_description,
                              p.price as p_price, p.count as p_count, p.manufacturer as p_manufacturer, p.content as image,
                              c.id as c_id, c.name as c_name
                              from product p, wishlist w, category c
                              where w.user_id = {userId}
                              and p.id = w.product_id
                              and c.id = p.category_id;";

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

            List<Product> products = new();

            foreach (DataRow row in dataTable.Rows)
            {
                Product product = new()
                {
                    id = row.Field<int>("p_id"),
                    name = row.Field<string>("p_name"),
                    description = row.Field<string>("p_description"),
                    price = row.Field<decimal>("p_price"),
                    count = row.Field<int>("p_count"),
                    manufacturer = row.Field<string>("p_manufacturer"),
                    isInWishlist = true,
                    image = row.Field<byte[]?>("image"),
                };

                product.category = new()
                {
                    id = row.Field<int>("c_id"),
                    name = row.Field<string>("c_name"),
                };

                products.Add(product);
            }

            return products;
        }
    }
}
