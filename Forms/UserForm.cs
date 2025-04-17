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
            SetupDataGridView(); // Setup columns first
            LoadUsers();         // Then fetch and display users
        }

        private void SetupDataGridView()
        {
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add("No", "No");
            dgvUsers.Columns.Add("Id", "Id");
            dgvUsers.Columns.Add("Name", "Name");
            dgvUsers.Columns.Add("Role", "Role");
            dgvUsers.Columns.Add("Phone", "Phone");
            dgvUsers.Columns.Add("Password", "Password");

            // Edit Button
            DataGridViewButtonColumn editButton = new DataGridViewButtonColumn();
            editButton.Name = "Edit";
            editButton.HeaderText = "Edit"; 
            editButton.Text = "Edit";
            editButton.UseColumnTextForButtonValue = true;
            dgvUsers.Columns.Add(editButton);

            // Delete Button
            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            deleteButton.Name = "Delete";
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            dgvUsers.Columns.Add(deleteButton);
        }

        private void LoadUsers()
        {
            userList = new Repository<User>().GetAll();
            DisplayUsers(userList);
        }

        private void DisplayUsers(List<User> users)
        {
            dgvUsers.Rows.Clear();
            int no = 1;
            foreach (var user in users)
            {
                dgvUsers.Rows.Add(no++, user.Id, user.Name, user.Role, user.Phone, user.Password);
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
            var filtered = userList.Where(u => u.Name.ToLower().Contains(keyword.ToLower())).ToList();
            DisplayUsers(filtered);
        }
        

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUsers.Columns[e.ColumnIndex].Name;
            int id = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["Id"].Value);

            Repository<User> repo = new Repository<User>();

            if (colName == "Edit") // Assuming you have a column named "Edit"
            {
                User existingUser = repo.GetAll().FirstOrDefault(u => u.Id == id);
                if (existingUser != null)
                {
                    UserModuleForm module = new UserModuleForm(existingUser);
                    module.FormClosed += (s, args) => LoadUsers();
                    module.ShowDialog();
                }
            }
            else if (colName == "Delete") // Assuming you have a column named "Delete"
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    repo.Delete(id);
                    MessageBox.Show("User deleted successfully!");
                    LoadUsers();
                }
            }
        }
    }
}
