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
            //SetupDataGridView();
            LoadProducts();
            ApplyRoleRestrictions();    
        }

        private void ApplyRoleRestrictions()
        {
            if (Session.Role != "Admin")
            {
                btnAdd.Enabled = false;

                // Hide the Edit and Delete columns if they exist
                if (dgvProducts.Columns.Contains("Edit"))
                    dgvProducts.Columns["Edit"].Visible = false;
                if (dgvProducts.Columns.Contains("Delete"))
                    dgvProducts.Columns["Delete"].Visible = false;
            }
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
                dgvProducts.Rows.Add(no++,p.Id, p.Name, p.Type, p.Category, p.Quantity, p.Price.ToString("0.00"));
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

            // Only Admins are allowed to edit or delete
            if ((colName == "Edit" || colName == "delete") && Session.Role != "Admin")
            {
                MessageBox.Show("Only admin is allowed to perform this action.");
                return;
            }

            if (colName == "Edit" || colName == "delete")
            {
                var cellValue = dgvProducts.Rows[e.RowIndex].Cells["Id"].Value;

                if (cellValue == null || !int.TryParse(cellValue.ToString(), out int id))
                {
                    MessageBox.Show("Invalid product ID.");
                    return;
                }

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
                else if (colName == "delete")
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
        
    }

