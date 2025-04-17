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

namespace BookStoreMG
{
    public partial class SellingForm : Form
    {
        public SellingForm()
        {
            InitializeComponent();
        }

        private void SellingForm_Load(object sender, EventArgs e)
        {
            ShowTotalSalesAndQuantity();
        }
        private void ShowTotalSalesAndQuantity()
        {
            try
            {
                // Get total sales and quantity first
                string salesQuery = "SELECT ISNULL(SUM(Total), 0), ISNULL(SUM(Quantity), 0) FROM SaleItems";
                string customersQuery = "SELECT COUNT(DISTINCT CustomerName) FROM Sales";  // or CustomerID if you have

                using (SqlCommand salesCmd = new SqlCommand(salesQuery, DatabaseConnection.Instance.Connection))
                {
                    using (SqlDataReader salesReader = salesCmd.ExecuteReader())
                    {
                        if (salesReader.Read())
                        {
                            decimal totalSales = salesReader.GetDecimal(0);
                            int totalQuantity = salesReader.GetInt32(1);

                            lblPrice.Text = $"Total: {totalSales:C}";
                            lblProducts.Text = $"Products Sold: {totalQuantity}";
                        }
                    }
                }

                // Now get total customers separately
                using (SqlCommand customersCmd = new SqlCommand(customersQuery, DatabaseConnection.Instance.Connection))
                {
                    int totalCustomers = (int)customersCmd.ExecuteScalar();
                    lblCustomers.Text = $"Customers: {totalCustomers}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
            finally
            {
                DatabaseConnection.Instance.Close();
            }
        }


        private void lblCustomers_Click(object sender, EventArgs e)
        {

        }

        private void lblProducts_Click(object sender, EventArgs e)
        {

        }

        private void lblPrice_Click(object sender, EventArgs e)
        {

        }
    }
}
