using BookStoreMG.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookStoreMG
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnExite_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection con = DatabaseConnection.Instance.Connection;
            string query = "SELECT * FROM Users WHERE Name=@name AND Password=@password";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", txtUsename.Text);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);

            SqlDataReader reader;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    UserRole loggedInUser = UserFactory.CreateUser(role);

                    MainForm mainForm = new MainForm(txtUsename.Text, loggedInUser.Role);
                    this.Hide();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials!");
                }
            }
            finally
            {
                DatabaseConnection.Instance.Close();
            }
        }

        private void txtUsename_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
