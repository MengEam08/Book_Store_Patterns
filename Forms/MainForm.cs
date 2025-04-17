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
            _userName = userName;  // store userName
            _userRole = role;      // store userRole

            lblUsername.Text = _userName;    // show Username
            lblRole.Text = _userRole;        // show Role
            LoadDashboard();
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
    }
}
