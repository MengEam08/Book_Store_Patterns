using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookStoreMG.Repositories;

namespace BookStoreMG.Forms
{
    public partial class CashProductForm : Form
    {
        public event Action<Product> OnProductSelected;

        private List<Product> productList;

        public CashProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            productList = new Repository<Product>().GetAll();
            DisplayProducts(productList);
        }

        private void DisplayProducts(List<Product> products)
        {
            dgvCashProducts.Rows.Clear();
            int no = 1;
            foreach (var p in products)
            {
                dgvCashProducts.Rows.Add(no++, p.Code, p.Name, p.Type, p.Category, p.Price);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = productList.Where(p => p.Name.ToLower().Contains(keyword)).ToList();
            DisplayProducts(filtered);
        }

        private void dgvCashProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedProduct = productList[e.RowIndex];
                OnProductSelected?.Invoke(selectedProduct);
                this.Close();
            }
        }
    }
}
