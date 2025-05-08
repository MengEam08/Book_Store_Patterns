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
    public partial class UserForm : Form
    {
        private List<User> userList = new List<User>();
       

       public UserForm()
        {
            InitializeComponent();
            LoadUsers();
            //ApplyRoleRestrictions();

        }

        private void LoadUsers()
        {
            userList = new Repository<User>().GetAll();
            DisplayUsers(userList);
            ApplyRoleRestrictions();
            //  ApplyRoleRestrictions();
        }

        private void ApplyRoleRestrictions()
        {
            if (Session.Role != "Admin")
            {
                btnAdd.Enabled = false;

                // Make sure columns exist before accessing them
                if (dgvUsers.Columns.Contains("Edit"))
                    dgvUsers.Columns["Edit"].Visible = false;

                if (dgvUsers.Columns.Contains("Delete"))
                    dgvUsers.Columns["Delete"].Visible = false;
            }
        }

        /*private void ApplyRoleRestrictions()
{
    if (Session.Role != "Admin")
    {
        btnAdd.Enabled = false;
        if (dgvUsers.Columns.Contains("Edit"))
            dgvUsers.Columns["Edit"].Visible = false;
        if (dgvUsers.Columns.Contains("Delete"))
            dgvUsers.Columns["Delete"].Visible = false;
    }
}
*/

        private void DisplayUsers(List<User> users)
        {
            dgvUsers.Rows.Clear();
            int no = 1;
            foreach (var user in users)
            {
                dgvUsers.Rows.Add(no++, user.Id, user.Name, user.Phone, user.Role, user.Password);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {

             UserModuleForm module = new UserModuleForm();
             module.FormClosed += (s, args) => LoadUsers(); // Refresh after closing
             module.ShowDialog();
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            var filtered = userList
                .Where(u => u.Name.ToLower().Contains(keyword.ToLower()))
                .ToList();
            DisplayUsers(filtered);
        }
        

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return; // prevent header click

            string colName = dgvUsers.Columns[e.ColumnIndex].Name;
            int id = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["userId"].Value);

            if (Session.Role != "Admin")
            {
                MessageBox.Show("You do not have permission to perform this action.");
                return;
            }

            Repository<User> repo = new Repository<User>();

            if (colName == "Edit")
            {
                User existingUser = repo.GetAll().FirstOrDefault(u => u.Id == id);
                if (existingUser != null)
                {
                    UserModuleForm module = new UserModuleForm(existingUser);
                    module.FormClosed += (s, args) => LoadUsers();
                    module.ShowDialog();
                }
            }
            else if (colName == "Delete")
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    repo.Delete(id);
                    MessageBox.Show("User deleted successfully!");
                    LoadUsers();
                }
            }

            /* if (e.RowIndex < 0) return; // prevent header click

             string colName = dgvUsers.Columns[e.ColumnIndex].Name;
             int id = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["userId"].Value);

             Repository<User> repo = new Repository<User>();

             if (colName == "Edit")
             {
                 User existingUser = repo.GetAll().FirstOrDefault(u => u.Id == id);
                 if (existingUser != null)
                 {
                     UserModuleForm module = new UserModuleForm(existingUser);
                     module.FormClosed += (s, args) => LoadUsers();
                     module.ShowDialog();
                 }
             }
             else if (colName == "Delete")
             {
                 var confirmResult = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo);
                 if (confirmResult == DialogResult.Yes)
                 {
                     repo.Delete(id);
                     MessageBox.Show("User deleted successfully!");
                     LoadUsers();
                 }
             }
             */
        }
    
    }
}
