using BookStoreMG.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStoreMG.Forms
{
    public partial class CustomerModuleForm : Form
    {
        private Repository<Customer> repo = new Repository<Customer>();
        private bool isEditMode = false;
        private int editingCustomerId = 0;

        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        public CustomerModuleForm(Customer customer) : this()
        {
            isEditMode = true;
            editingCustomerId = customer.Id;
            txtName.Text = customer.Name;
            txtAddress.Text = customer.Address;
            txtPhone.Text = customer.Phone;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer
            {
                Id = editingCustomerId,
                Name = txtName.Text,
                Address = txtAddress.Text,
                Phone = txtPhone.Text
            };

            if (isEditMode)
                repo.Update(customer);
            else
                repo.Save(customer);

            MessageBox.Show("Customer saved!");
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (editingCustomerId != 0)
            {
                repo.Delete(editingCustomerId);
                MessageBox.Show("Customer deleted.");
            }
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
