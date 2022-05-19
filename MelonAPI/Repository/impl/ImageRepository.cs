using MelonAPI.Model.exception;
using Npgsql;
using System.Data;

namespace MelonAPI.Repository.impl
{
    public class ImageRepository : IImageRepository
    {

        private readonly IConfiguration configuration;

        public ImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Byte[] DownloadImage(int imageId)
        {
            string query = $"select content from image where id = {imageId}";

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

            if (dataTable.Rows == null || dataTable.Rows.Count == 0)
            {
                throw new IdNotFoundException($"Image with ID {imageId} not found");
            }

            DataRow dataRow = dataTable.Rows[0];

            if (dataRow == null)
            {
                throw new IdNotFoundException($"Image with ID {imageId} not found");
            }

            Byte[] content = dataRow.Field<byte[]>("content");

            if (content == null)
            {
                return Array.Empty<byte>();
            }

            return content;
        }

        public void UploadImage(byte[] image)
        {

            string query = $"insert into image (content) values (@image);";

            string sqlDataSource = configuration.GetConnectionString("MelonAppCon");

            using NpgsqlConnection con = new(sqlDataSource);
            con.Open();
            using NpgsqlCommand command = new(query, con);
            
            var parameter = command.CreateParameter();
            parameter.ParameterName = "image";
            parameter.Value = image;

            command.Parameters.Add(parameter);

            command.ExecuteReader();

            con.Close();

        }
    }
}
