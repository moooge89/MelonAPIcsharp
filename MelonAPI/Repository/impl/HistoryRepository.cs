using MelonAPI.Model;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class HistoryRepository : IHistoryRepository
    {

        private readonly IConfiguration configuration;

        public HistoryRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Dictionary<int, List<ProductLight>> LoadHistories(int userId)
        {
            string query = @$"select history_id, h.product_id as product_id, p.name as p_name, price, category_id, p.content as image,
                              (select exists (select * from wishlist w where w.product_id = h.product_id and w.user_id = {userId})) as is_wishlist,
                              (select exists (select * from cart where cart.product_id = h.product_id and cart.user_id = {userId})) as is_cart
                              from history h, product p
                              where h.user_id = {userId}
                              and h.product_id = p.id;";

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

            Dictionary<int, List<ProductLight>> dictionary = new Dictionary<int, List<ProductLight>>();

            foreach (DataRow row in dataTable.Rows)
            {

                int historyId = row.Field<int>("history_id");

                ProductLight productLight = new()
                {
                    id = row.Field<int?>("product_id"),
                    name = row.Field<string?>("p_name"),
                    price = row.Field<int?>("price"),
                    categoryId = row.Field<int?>("category_id"),
                    isInWishlist = row.Field<bool?>("is_wishlist"),
                    isInCart = row.Field<bool?>("is_cart"),
                    image = row.Field<byte[]?>("image"),
                };

                List<ProductLight> list = dictionary.GetValueOrDefault(historyId, new List<ProductLight>());
                list.Add(productLight);
                dictionary.TryAdd(historyId, list);
            }

            return dictionary;
        }
    }
}
