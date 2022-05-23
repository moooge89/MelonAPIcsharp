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
            string query = @$"select p.id as id, p.name as name, p.price as price, p.category_id as category_id, p.content as image,
                           (select exists(select * from wishlist w where w.product_id = p.id and w.user_id = {userId})) as is_wishlist,
                           (select exists(select * from cart where cart.product_id = p.id and cart.user_id = {userId})) as is_cart
                           from product p
                           where category_id = {CategoryId};";

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
                    id = row.Field<int>("id"),
                    name = row.Field<string>("name"),
                    price = row.Field<decimal>("price"),
                    categoryId = row.Field<int>("category_id"),
                    isInCart = row.Field<bool>("is_cart"),
                    isInWishlist = row.Field<bool>("is_wishlist"),
                    image = row.Field<byte[]?>("image"),
                };
                products.Add(product);
            }

            return products;
        }

        public Product LoadProductById(int productId, int userId)
        {
            string query = @$"select p.id as product_id, p.name as product_name,
                              p.description, p.price, p.count, p.manufacturer,
                              p.category_id, c.id as category_id, c.name as category_name,
                              p.content as image,
                              (select exists (select * from wishlist w where w.product_id = p.id and w.user_id = {userId})) as is_wishlist,
                              (select exists (select * from cart where cart.product_id = p.id and cart.user_id = {userId})) as is_cart
                              from product p, category c
                              where p.category_id = c.id
                              and p.id = {productId};";

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
                id = dataTable.Rows[0].Field<int>("product_id"),
                name = dataTable.Rows[0].Field<string>("product_name"),
                description = dataTable.Rows[0].Field<string>("description"),
                price = dataTable.Rows[0].Field<decimal>("price"),
                count = dataTable.Rows[0].Field<int>("count"),
                manufacturer = dataTable.Rows[0].Field<string>("manufacturer"),
                isInWishlist = dataTable.Rows[0].Field<bool>("is_wishlist"),
                isInCart = dataTable.Rows[0].Field<bool>("is_cart"),
                image = dataTable.Rows[0].Field<byte[]?>("image"),
            };

            Category category = new()
            {
                id = dataTable.Rows[0].Field<int>("category_id"),
                name = dataTable.Rows[0].Field<string>("category_name"),
            };

            product.category = category;

            return product;
        }

        public Product SaveProduct(Product product)
        {
            string query = @$"insert into product (name, description, price, count, manufacturer, category_id, content) values
                           ('{product.name}', '{product.description}', {product.price}, {product.count}, '{product.manufacturer}',
                           {product.category?.id}, @image)
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
                if (product.image != null)
                {
                    parameter.Value = product.image;
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

            product.id = dataRow.Field<int>("id");

            return product;
        }

        public Product UpdateProduct(int id, Product product)
        {
            string query = @$"update product set name = {product.name}, description = {product.description}, price = {product.price}, 
                           count = {product.count}, manufacturer = {product.manufacturer}, category_id = {product.category?.id}, set content = @image
                           where id = {id};";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using (NpgsqlConnection con = new(sqlDataSource))
            {
                con.Open();

                using NpgsqlCommand command = new(query, con);

                var parameter = command.CreateParameter();
                parameter.ParameterName = "image";
                if (product.image != null)
                {
                    parameter.Value = product.image;
                }
                else
                {
                    parameter.Value = DBNull.Value;
                }

                command.Parameters.Add(parameter);

                con.Close();
            }

            product.id = id;
            return product;
        }

        public void DeleteProduct(int id)
        {
            string query = $"delete from product where id = {id};";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using NpgsqlConnection con = new(sqlDataSource);
            con.Open();

            using NpgsqlCommand command = new(query, con);

            command.ExecuteNonQuery();

            con.Close();
        }

        public List<ProductLight> LoadProductWithFilter(ProductFilter? filter, int userId)
        {

            if (filter == null || filter.categoryId == null)
            {
                throw new NullReferenceException("Filter is null");
            }

            List<ProductLight> products = LoadProductByCategoryId((int)filter.categoryId, userId);

            if (filter.priceFrom == null)
            {
                filter.priceFrom = 0;
            }

            if (filter.priceTo == null)
            {
                filter.priceTo = 1000;
            }

            return products.Where(p => p.price >= filter.priceFrom && p.price <= filter.priceTo).ToList();
        }
    }
}
