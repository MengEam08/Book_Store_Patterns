using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BookStoreMG.Repositories;
namespace BookStoreMG.Forms
{
    public partial class ProductForm : Form
    {
        private List<Product> productList = new List<Product>();
        private ISearchStrategy searchStrategy;

        public ProductForm()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadProducts();
        }

        private void SetupDataGridView()
        {
            dgvProducts.Columns.Clear();
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.RowTemplate.Height = 40;

            dgvProducts.Columns.Add("No", "No");
            dgvProducts.Columns.Add("Id", "Id");
            dgvProducts.Columns.Add("Name", "Name");
            dgvProducts.Columns.Add("Type", "Type");
            dgvProducts.Columns.Add("Category", "Category");
            dgvProducts.Columns.Add("Price", "Price");

            // Edit Button
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
            editButton.Name = "Edit";
            editButton.HeaderText = "Edit";
            editButton.Text = "Edit";
            editButton.UseColumnTextForButtonValue = true;
            dgvProducts.Columns.Add(editButton);

            // Delete Button
            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            deleteButton.Name = "Delete";
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            dgvProducts.Columns.Add(deleteButton);
        }

        private void LoadProducts()
        {
            productList = new Repository<Product>().GetAll();
            DisplayProducts(productList);
        }

        private void DisplayProducts(List<Product> products)
        {
            dgvProducts.Rows.Clear();
            int no = 1;
            foreach (var p in products)
            {
                dgvProducts.Rows.Add(no++, p.Id, p.Name, p.Type, p.Category, p.Price.ToString("0.00"));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            ProductModuleForm module = new ProductModuleForm();
            module.FormClosed += (s, args) => LoadProducts();
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            string keyword = txtSearch.Text;
            var filtered = productList
                .Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
                .ToList();

            DisplayProducts(filtered);
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore header clicks

            string colName = dgvProducts.Columns[e.ColumnIndex].Name;
            int id = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["Id"].Value);

            Repository<Product> repo = new Repository<Product>();

            if (colName == "Edit")
            {
                Product existingProduct = repo.GetAll().FirstOrDefault(p => p.Id == id);
                if (existingProduct != null)
                {
                    ProductModuleForm module = new ProductModuleForm(existingProduct);
                    module.FormClosed += (s, args) => LoadProducts();
                    module.ShowDialog();
                }
            }
            else if (colName == "Delete")
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    repo.Delete(id);
                    MessageBox.Show("Product deleted successfully!");
                    LoadProducts();
                }
            }
        }
    }
}
