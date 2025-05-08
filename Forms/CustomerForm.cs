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
    public partial class CustomerForm : Form
    {
        private List<Customer> customerList = new List<Customer>();

        public CustomerForm()
        {
            InitializeComponent();
            //SetupDataGridView();
            LoadCustomers();
            ApplyRoleRestrictions();
        }
        private void ApplyRoleRestrictions()
        {
            if (Session.Role != "Admin")
            {
                btnAdd.Enabled = false;

                // Hide Edit and Delete columns if they exist
                if (dgvCustomers.Columns.Contains("Edit"))
                    dgvCustomers.Columns["Edit"].Visible = false;
                if (dgvCustomers.Columns.Contains("Delete"))
                    dgvCustomers.Columns["Delete"].Visible = false;
            }
        }
        private void LoadCustomers()
        {
            customerList = new Repository<Customer>().GetAll();
            DisplayCustomers(customerList);
        }

        private void DisplayCustomers(List<Customer> customers)
        {
            dgvCustomers.Rows.Clear();
            int no = 1;
            foreach (var c in customers)
            {
                dgvCustomers.Rows.Add(no++, c.Id, c.Name, c.Address, c.Phone);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            var filtered = customerList.Where(c => c.Name.ToLower().Contains(keyword.ToLower())).ToList();
            DisplayCustomers(filtered);
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvCustomers.Columns[e.ColumnIndex].Name;
            int id = Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells["CustId"].Value);

            // Restrict edit/delete to Admin
            if ((colName == "Edit" || colName == "Delete") && Session.Role != "Admin")
            {
                MessageBox.Show("Only admin is allowed to perform this action.");
                return;
            }

            Repository<Customer> repo = new Repository<Customer>();

            if (colName == "Edit")
            {
                Customer existingCustomer = repo.GetAll().FirstOrDefault(c => c.Id == id);
                if (existingCustomer != null)
                {
                    CustomerModuleForm module = new CustomerModuleForm(existingCustomer);
                    module.FormClosed += (s, args) => LoadCustomers();
                    module.ShowDialog();
                }
            }
            else if (colName == "Delete")
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    repo.Delete(id);
                    MessageBox.Show("Customer deleted successfully!");
                    LoadCustomers();
                }
            }


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Session.Role != "Admin")
            {
                MessageBox.Show("Only admin is allowed to add customers.");
                return;
            }

            CustomerModuleForm module = new CustomerModuleForm();
            module.FormClosed += (s, args) => LoadCustomers();
            module.ShowDialog();

            /* CustomerModuleForm module = new CustomerModuleForm();
             module.FormClosed += (s, args) => LoadCustomers();
             module.ShowDialog();
            */
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            var filtered = customerList.Where(c => c.Name.ToLower().Contains(keyword.ToLower())).ToList();
            DisplayCustomers(filtered);
        }
    }
}
