 using Isi.ShoppingApp.Core.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isi.ShoppingApp.Data.Repositories
{
    public class ProductRepository
    {
        private readonly string connectionString;

        public ProductRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ShoppingAppDatabase"].ConnectionString;
        }

        //QUERY METHODS: ----------------------------------------------------------
        public bool ProductExists(long id)
        {
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(Id) FROM dbo.Products WHERE Id = @Id";
            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;

            int count = (int)command.ExecuteScalar();
            return count > 0;
        }

        public Product GetProduct(long id)
        {
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT" +
                "[Id], " +
                "[Name], " +
                "[Category], " +
                "[Description], " +
                "[Price], " +
                "[Quantity], " +
                "[PercentageDiscount] " +
                "FROM dbo.Products " +
                "WHERE Id = @Id";

            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
                return ReadNextProduct(reader);

            return null;
        }

        public List<Product>GetProducts()
        {
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT " +
                "[Id], " +
                "[Name], " +
                "[Category], " +
                "[Description], " +
                "[Price], " +
                "[Quantity], " +
                "[PercentageDiscount] " +
                "FROM dbo.Products";

            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();

            while(reader.Read())
            {
                products.Add(ReadNextProduct(reader));
            }

            return products;
        }

        public List<Product> GetAvailableProducts()
        {
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT " +
                "Id, " +
                "Name, " +
                "Category, " +
                "Description, " +
                "Price, " +
                "Quantity, " +
                "PercentageDiscount " +
                "FROM dbo.Products " + 
                "WHERE Quantity > 0";

            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();

            while (reader.Read())
            {
                products.Add(ReadNextProduct(reader));
            }

            return products;
        }

        public List<Product> GetUnavailableProducts()
        {
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT " +
                "[Id], " +
                "[Name], " +
                "[Category], " +
                "[Description], " +
                "[Price], " +
                "[Quantity], " +
                "[PercentageDiscount] " +
                "FROM dbo.Products " +
                "WHERE Quantity = 0";
            SqlDataReader reader = command.ExecuteReader();

            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                products.Add(ReadNextProduct(reader));
            }
            return products;
        }


        //NON-QUERY METHODS: ----------------------------------------------------------
        public Product AddProduct(Product product)
        {
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO dbo.Products " +
                "(Name, Category, Description, Price, Quantity, PercentageDiscount, Created, Updated) " +
                "OUTPUT INSERTED.Id " +
                "VALUES " +
                "(@Name, @Category, @Description, @Price, @Quantity, @PercentageDiscount, @Created, @Updated)";

            DateTime now = DateTime.Now;
            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = product.Name;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = product.Category;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = product.Description;
            command.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = product.Quantity;
            command.Parameters.Add("@PercentageDiscount", SqlDbType.Decimal).Value = product.PercentageDiscount ?? DBNull.Value as object;
            command.Parameters.Add("@Created", SqlDbType.DateTime2).Value = now;
            command.Parameters.Add("@Updated", SqlDbType.DateTime2).Value = now;

            long id = (long)command.ExecuteScalar();
            return new Product(id, product.Name, product.Category, product.Description, product.Price, product.Quantity, product.PercentageDiscount);
        }

        public bool RemoveProduct(Product product)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM dbo.Products WHERE Id = @Id";
            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = product.Id;

            int rowsAffected = 0;
            rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public bool RemoveProduct(long id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM dbo.Products WHERE Id = @Id";
            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;

            int rowsAffected = 0;
            rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        public bool UpdateProduct(Product product)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE dbo.Products SET Name = @Name, Category = @Category, Description = @Description, Price = @Price, Quantity=@Quantity, PercentageDiscount=@PercentageDiscount, Updated=@Updated " +
                "WHERE Id = @Id";

            command.Parameters.Add("@Id", SqlDbType.BigInt).Value = product.Id;

            command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = product.Name;
            command.Parameters.Add("@Category", SqlDbType.NVarChar).Value = product.Category;
            command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = product.Description;
            command.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
            command.Parameters.Add("@Quantity", SqlDbType.Decimal).Value = product.Quantity;
            command.Parameters.Add("@PercentageDiscount", SqlDbType.Decimal).Value = product.PercentageDiscount ?? DBNull.Value as object;
            command.Parameters.Add("@Updated", SqlDbType.DateTime2).Value = DateTime.Now;

            int rowsAffected = 0;
            rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }







        private Product ReadNextProduct(SqlDataReader reader)
        {
            long id = reader.GetInt64(0);
            string name = reader.GetString(1);
            string category = reader.GetString(2);
            string description = reader.GetString(3);
            decimal price = reader.GetDecimal(4);
            int quantity = reader.GetInt32(5);
            decimal? percentageDiscount = reader.IsDBNull(6) ? null : reader.GetDecimal(6);

            return new Product(id, name, category, description, price, quantity, percentageDiscount);
        }
    }
}
