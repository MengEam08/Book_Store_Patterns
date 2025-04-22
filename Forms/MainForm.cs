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
    public partial class MainForm : Form
    {
        private string _userRole;
        private string _userName;

        public MainForm(string userName, string role)
        {
            InitializeComponent();
            _userName = userName;  // Store userName
            _userRole = role;      // Store userRole

            lblUsername.Text = _userName;    // Display Username
            lblRole.Text = _userRole;
        }


        private void LoadDashboard()
        {
            
        }

        private void btnSelling_Click(object sender, EventArgs e)
        {
            SellingForm sf = new SellingForm();
            LoadFormInPanel(sf);
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            UserForm uf = new UserForm();
            LoadFormInPanel(uf);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            CustomerForm cf = new CustomerForm();
            LoadFormInPanel(cf);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            ProductForm pf = new ProductForm();
            LoadFormInPanel(pf);
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            CashForm cf = new CashForm(_userRole);
            LoadFormInPanel(cf);
        }
        private void LoadFormInPanel(Form form)
        {
            panelMain.Controls.Clear();
            form.TopLevel = false;
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lblUsername_Click(object sender, EventArgs e)
        {

        }

        private void lblRole_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide(); // hide current MainForm
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}
