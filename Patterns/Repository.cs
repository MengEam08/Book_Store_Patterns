using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BookStoreMG.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\OOAD\S2 Project\MiniStore\BookStoreMG\MyBook.mdf;Integrated Security=True";

        public void Save(T entity)
        {
            if (typeof(T) == typeof(Customer))
                SaveCustomer(entity as Customer);
            else if (typeof(T) == typeof(Product))
                SaveProduct(entity as Product);
            else if (typeof(T) == typeof(User))
                SaveUser(entity as User);
            else
                throw new NotSupportedException("Save not supported for this type.");
        }

        public void Update(T entity)
        {
            if (typeof(T) == typeof(Customer))
                UpdateCustomer(entity as Customer);
            else if (typeof(T) == typeof(Product))
                UpdateProduct(entity as Product);
            else if (typeof(T) == typeof(User))
                UpdateUser(entity as User);
            else
                throw new NotSupportedException("Update not supported for this type.");
        }

        public void Delete(int id)
        {
            if (typeof(T) == typeof(Customer))
                DeleteCustomer(id);
            else if (typeof(T) == typeof(Product))
                DeleteProduct(id);
            else if (typeof(T) == typeof(User))
                DeleteUser(id);
            else
                throw new NotSupportedException("Delete not supported for this type.");
        }

        public List<T> GetAll()
        {
            if (typeof(T) == typeof(Customer))
                return GetAllCustomers() as List<T>;
            else if (typeof(T) == typeof(Product))
                return GetAllProducts() as List<T>;
            else if (typeof(T) == typeof(User))
                return GetAllUsers() as List<T>;
            else
                throw new NotSupportedException("GetAll not supported for this type.");
        }

        // -------------------- CUSTOMER METHODS --------------------
        private void SaveCustomer(Customer customer)
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

        private void UpdateCustomer(Customer customer)
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

        private void DeleteCustomer(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        private List<Customer> GetAllCustomers()
        {
            var list = new List<Customer>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Customers", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Customer
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Address = reader["Address"].ToString(),
                        Phone = reader["Phone"].ToString()
                    });
                }
            }

            return list;
        }

        // -------------------- PRODUCT METHODS --------------------
        private void SaveProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Products (Name, Type, Category, Quantity, Price) VALUES (@Name, @Type, @Category, @Quantity, @Price)",
                    connection);

                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Type", product.Type);
                command.Parameters.AddWithValue("@Category", product.Category);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Price", product.Price);

                command.ExecuteNonQuery();
            }
        }

        private void UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Products SET Name = @Name, Type = @Type, Category = @Category, Quantity = @Quantity, Price = @Price WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Type", product.Type);
                command.Parameters.AddWithValue("@Category", product.Category);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Price", product.Price);

                command.ExecuteNonQuery();
            }
        }

        private void DeleteProduct(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        private List<Product> GetAllProducts()
        {
            var list = new List<Product>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Products", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Product
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Type = reader["Type"].ToString(),
                        Category = reader["Category"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"])
                    });
                }
            }

            return list;
        }

        // -------------------- USER METHODS --------------------
        private void SaveUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Users (Name, Phone, Role, Password) VALUES (@Name, @Phone, @Role, @Password)",
                    connection);

                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Password", user.Password);

                command.ExecuteNonQuery();
            }
        }

        private void UpdateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Users SET Name = @Name, Phone = @Phone, Role = @Role, Password = @Password WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Password", user.Password);

                command.ExecuteNonQuery();
            }
        }

        private void DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        private List<User> GetAllUsers()
        {
            var list = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new User
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Role = reader["Role"].ToString(),
                        Password = reader["Password"].ToString()
                    });
                }
            }

            return list;
        }
    }
}
