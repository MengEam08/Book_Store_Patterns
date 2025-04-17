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
            SetupDataGridView();
            LoadCustomers();
        }

        private void SetupDataGridView()
        {
            dgvCustomers.Columns.Clear();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.RowTemplate.Height = 40;

            dgvCustomers.Columns.Add("No", "No");
            dgvCustomers.Columns.Add("Id", "Id");
            dgvCustomers.Columns.Add("Name", "Name");
            dgvCustomers.Columns.Add("Address", "Address");
            dgvCustomers.Columns.Add("Phone", "Phone");

            // Edit Button
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
            editButton.Name = "Edit";
            editButton.HeaderText = "Edit";
            editButton.Text = "Edit";
            editButton.UseColumnTextForButtonValue = true;
            dgvCustomers.Columns.Add(editButton);

            // Delete Button
            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            deleteButton.Name = "Delete";
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            dgvCustomers.Columns.Add(deleteButton);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModuleForm module = new CustomerModuleForm();
            module.FormClosed += (s, args) => LoadCustomers();
            module.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            var filtered = customerList.Where(c => c.Name.ToLower().Contains(keyword.ToLower())).ToList();
            DisplayCustomers(filtered);
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignore header clicks

            string colName = dgvCustomers.Columns[e.ColumnIndex].Name;
            int id = Convert.ToInt32(dgvCustomers.Rows[e.RowIndex].Cells["Id"].Value);

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
    }
}
