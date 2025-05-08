using BookStoreMG.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace BookStoreMG.Forms
{
    public partial class ProductModuleForm : Form
    {
        private Repository<Product> repo = new Repository<Product>();
        private bool isEditMode = false;
        private int editingProductId = 0;

        public ProductModuleForm()
        {
            InitializeComponent();
        }

        public ProductModuleForm(Product product) : this()
        {
            isEditMode = true;
            editingProductId = product.Id;
            txtName.Text = product.Name;
            txtType.Text = product.Type;
            txtCategory.Text = product.Category;
            txtQty.Text = product.Quantity.ToString();
            txtPrice.Text = product.Price.ToString();
        }


        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Product product = new Product
            {
                Id = editingProductId,
                Name = txtName.Text,
                Type = txtType.Text,
                Category = txtCategory.Text,
                Quantity = int.Parse(txtQty.Text),
                Price = decimal.Parse(txtPrice.Text)
            };

            if (isEditMode)
                repo.Update(product);
            else
                repo.Save(product);

            MessageBox.Show("Product saved!");
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (editingProductId != 0)
            {
                repo.Delete(editingProductId);
                MessageBox.Show("Product deleted.");
            }
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
