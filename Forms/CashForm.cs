using BookStoreMG.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStoreMG.Forms
{
    public partial class CashForm : Form
    {
        private List<Product> selectedProducts = new List<Product>();
        private Customer selectedCustomer;
        private string cashierRole;
        private SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\OOAD\S2 Project\MiniStore\BookStoreMG\MyBook.mdf;Integrated Security=True");
        private string title = "Book Store Cash System";
        private Repository<Product> productRepo = new Repository<Product>();

        public CashForm(string role)
        {
            InitializeComponent();
            cashierRole = role;
            lblCashier.Text = $"Cashier: {cashierRole}";
            //SetupDataGridView();
            LoadCash();
        }

       /* private void SetupDataGridView()
        {
            dgvCart.Columns.Clear();
            dgvCart.AutoGenerateColumns = false;
            dgvCart.RowTemplate.Height = 40;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.MultiSelect = false;

            dgvCart.Columns.Add("No", "No");
            dgvCart.Columns.Add("ProductName", "Name");
            dgvCart.Columns.Add("Price", "Price");
            dgvCart.Columns.Add("Quantity", "Qty");
            dgvCart.Columns.Add("Total", "Total");

            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn
            {
                Name = "Edit",
                HeaderText = "Edit Qty",
                Text = "Edit",
                UseColumnTextForButtonValue = true
            };
            dgvCart.Columns.Add(editButton);

            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn
            {
                Name = "Delete",
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true
            };
            dgvCart.Columns.Add(deleteButton);
        }
        */
        private void LoadSelectedProducts()
        {
            dgvCart.Rows.Clear();
            int no = 1;
            decimal total = 0;

            foreach (var p in selectedProducts)
            {
                decimal lineTotal = p.Price * p.Quantity;
                dgvCart.Rows.Add(no++, p.Name, p.Price.ToString("F2"), p.Quantity, lineTotal.ToString("F2"));
                total += lineTotal;
            }

            lblTotal.Text = $"Total: ${total:F2}";
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashProductForm cpf = new CashProductForm();
            cpf.OnProductSelected += product =>
            {
                selectedProducts.Add(product);
                LoadSelectedProducts();
            };
            cpf.ShowDialog();
        }



        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvCart.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to remove this product?", "Remove Product",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    selectedProducts.RemoveAt(e.RowIndex);
                    LoadSelectedProducts();
                }
            }
            else if (colName == "Edit")
            {
                var product = selectedProducts[e.RowIndex];
                string input = Prompt($"Enter new quantity for {product.Name}:", "Edit Quantity", product.Quantity.ToString());

                if (int.TryParse(input, out int newQty))
                {
                    int availableQty = CheckProductQuantity(product.Name);
                    if (newQty > 0 && newQty <= availableQty)
                    {
                        selectedProducts[e.RowIndex].Quantity = newQty;
                        LoadSelectedProducts();
                    }
                    else
                    {
                        MessageBox.Show($"Invalid quantity. Available stock: {availableQty}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            /* if (e.RowIndex < 0) return;
             string colName = dgvCart.Columns[e.ColumnIndex].Name;

             if (colName == "Delete")
             {
                 if (MessageBox.Show("Are you sure you want to remove this product?", "Remove Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                 {
                     selectedProducts.RemoveAt(e.RowIndex);
                     LoadSelectedProducts();
                 }
             }
             else if (colName == "Edit")
             {
                 var product = selectedProducts[e.RowIndex];
                 string input = Prompt($"Enter new quantity for {product.Name}:", "Edit Quantity", product.Quantity.ToString());

                 if (int.TryParse(input, out int newQty))
                 {
                     int availableQty = CheckProductQuantity(product.Name);
                     if (newQty > 0 && newQty <= availableQty)
                     {
                         selectedProducts[e.RowIndex].Quantity = newQty;
                         LoadSelectedProducts();
                     }
                     else
                     {
                         MessageBox.Show($"Invalid quantity. Available stock: {availableQty}");
                     }
                 }
             }
            */
        }
        private int CheckProductQuantity(string productName)
        {
            var products = productRepo.GetAll();
            foreach (var p in products)
            {
                if (p.Name == productName)
                    return p.Quantity;
            }
            return 0;
        }

        private void LoadCash()
        {
            // Initially nothing to load because cart is fresh per new checkout
            dgvCart.Rows.Clear();
            lblTotal.Text = "Total: $0.00";
            lblCustomerName.Text = "Customer: None";
        }

        // Custom input box (no need Microsoft.VisualBasic)
        private string Prompt(string text, string caption, string defaultValue = "")
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 170,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, Width = 340 };
            TextBox inputBox = new TextBox() { Left = 20, Top = 50, Width = 340, Text = defaultValue };
            Button confirmation = new Button() { Text = "OK", Left = 260, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : defaultValue;
        }
        private void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            CashCustomerForm ccf = new CashCustomerForm();
            ccf.OnCustomerSelected += customer =>
            {
                selectedCustomer = customer;
                lblCustomerName.Text = $"Customer: {customer.Name}";
            };
            ccf.ShowDialog();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }
            if (selectedProducts.Count == 0)
            {
                MessageBox.Show("Please add at least one product.");
                return;
            }

            try
            {
                cn.Open();
                SqlTransaction transaction = cn.BeginTransaction();

                // Insert into Sales table first
                int saleId = InsertSale(transaction);

                // Insert each product into SaleItems table
                foreach (var product in selectedProducts)
                {
                    InsertSaleItem(transaction, saleId, product);
                }

                transaction.Commit();

                // Calculate total price before clearing products
                decimal totalPrice = selectedProducts.Sum(p => p.Price * p.Quantity);

                MessageBox.Show($"Checkout complete!\n" +
                                $"Customer: {selectedCustomer.Name}\n" +
                                $"Total items: {selectedProducts.Count}\n" +
                                $"Total price: ${totalPrice:F2}");

                // Reset cart and UI
                selectedProducts.Clear();
                selectedCustomer = null;
                dgvCart.Rows.Clear();
                lblTotal.Text = "Total: $0.00";
                lblCustomerName.Text = "Customer: None";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Checkout failed: {ex.Message}");
            }
            finally
            {
                cn.Close();
            }

        }
        private int InsertSale(SqlTransaction transaction)
        {
            int saleId = 0;
            decimal totalAmount = selectedProducts.Sum(p => p.Price * p.Quantity); // Calculate total

            string insertSaleQuery = @"
        INSERT INTO Sales (CustomerName, Cashier, TotalAmount, SaleDate)
        OUTPUT INSERTED.Id
        VALUES (@CustomerName, @Cashier, @TotalAmount, @SaleDate)";

            using (SqlCommand cmd = new SqlCommand(insertSaleQuery, cn, transaction))
            {
                cmd.Parameters.AddWithValue("@CustomerName", selectedCustomer.Name);
                cmd.Parameters.AddWithValue("@Cashier", cashierRole); // insert cashier
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount); // insert total
                cmd.Parameters.AddWithValue("@SaleDate", DateTime.Now);

                saleId = (int)cmd.ExecuteScalar();
            }
            return saleId;
        }


        private void InsertSaleItem(SqlTransaction transaction, int saleId, Product product)
        {
            string insertItemQuery = "INSERT INTO SaleItems (SaleId, ProductName, Quantity, Price, Total) VALUES (@SaleId, @ProductName, @Quantity, @Price, @Total)";
            using (SqlCommand cmd = new SqlCommand(insertItemQuery, cn, transaction))
            {
                cmd.Parameters.AddWithValue("@SaleId", saleId);
                cmd.Parameters.AddWithValue("@ProductName", product.Name);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Total", product.Price * product.Quantity);

                cmd.ExecuteNonQuery();
            }
        }

        private void SaveSale()
        {
            cn.Open();
            SqlTransaction transaction = cn.BeginTransaction();

            try
            {
                foreach (var product in selectedProducts)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO SaleItems (CustomerId, ProductName, Price, Quantity) VALUES (@CustomerId, @ProductName, @Price, @Quantity)", cn, transaction);
                    cmd.Parameters.AddWithValue("@CustomerId", selectedCustomer.Id);
                    cmd.Parameters.AddWithValue("@ProductName", product.Name);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.ExecuteNonQuery();

                    // Optionally decrease product stock
                    SqlCommand updateStock = new SqlCommand("UPDATE Products SET Quantity = Quantity - @Quantity WHERE Name = @Name", cn, transaction);
                    updateStock.Parameters.AddWithValue("@Quantity", product.Quantity);
                    updateStock.Parameters.AddWithValue("@Name", product.Name);
                    updateStock.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception)
            {   
                transaction.Rollback();
                throw;
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
