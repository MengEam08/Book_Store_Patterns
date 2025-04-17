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
    public partial class CashCustomerForm : Form
    {
        public event Action<Customer> OnCustomerSelected;

        private List<Customer> customerList;

        public CashCustomerForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            customerList = new Repository<Customer>().GetAll();
            DisplayCustomers(customerList);
        }

        private void DisplayCustomers(List<Customer> customers)
        {
            dgvCashCustomers.Rows.Clear();
            int no = 1;
            foreach (var c in customers)
            {
                dgvCashCustomers.Rows.Add(no++, c.Id, c.Name, c.Phone);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = customerList.Where(c => c.Name.ToLower().Contains(keyword)).ToList();
            DisplayCustomers(filtered);
        }

        private void dgvCashCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                var selectedCustomer = customerList[e.RowIndex];
                OnCustomerSelected?.Invoke(selectedCustomer);
                this.Close();
            }
        }
    }
}
