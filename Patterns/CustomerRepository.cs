using System;
using System.Data.SqlClient;
using System.Configuration;

namespace BookStoreMG.Patterns
{
    internal class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["BookStoreDB"].ConnectionString;
        }

        public void Save(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Customers (Name, Address, Phone) VALUES (@Name, @Address, @Phone)",
                    connection);

                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@Phone", customer.Phone);

                command.ExecuteNonQuery();
            }
        }

        public void Update(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Customers SET Name = @Name, Address = @Address, Phone = @Phone WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", customer.Id);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Address", customer.Address);
                command.Parameters.AddWithValue("@Phone", customer.Phone);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
