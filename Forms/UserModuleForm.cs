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
    public partial class UserModuleForm : Form
    {
        private Repository<User> repo = new Repository<User>();
        private bool isEditMode = false;
        private int editingUserId = 0;

        public UserModuleForm()
        {
            InitializeComponent();
        }

        public UserModuleForm(User existingUser) : this()
        {
            isEditMode = true;
            editingUserId = existingUser.Id;
            txtName.Text = existingUser.Name;
            txtPhone.Text = existingUser.Phone;
            txtRole.Text = existingUser.Role;
            txtPassword.Text = existingUser.Password;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                Id = editingUserId,
                Name = txtName.Text,
                Phone = txtPhone.Text,
                Role = txtRole.Text,
                Password = txtPassword.Text
            };

            if (isEditMode)
                repo.Update(user);
            else
                repo.Save(user);

            MessageBox.Show("User saved successfully!");
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (editingUserId != 0)
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    repo.Delete(editingUserId);
                    MessageBox.Show("User deleted.");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Cannot delete a user that doesn't exist yet!");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                if (editingUserId != 0)
                {
                    User user = new User
                    {
                        Id = editingUserId,
                        Name = txtName.Text,
                        Phone = txtPhone.Text,
                        Role = txtRole.Text,
                        Password = txtPassword.Text
                    };

                    repo.Update(user);
                    MessageBox.Show("User updated successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No user selected to update!");
                }
            }
        }
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtRole.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
