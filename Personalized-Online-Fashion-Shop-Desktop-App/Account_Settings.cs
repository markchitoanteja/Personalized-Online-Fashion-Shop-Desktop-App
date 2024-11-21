using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Mysqlx.Expect.Open.Types;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class Account_Settings : Form
    {
        private bool is_busy = false;

        public Account_Settings()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (!is_busy)
            {
                Close();
            }
        }

        private void btn_close_2_Click(object sender, EventArgs e)
        {
            if (!is_busy)
            {
                Close();
            }
        }

        private async void btn_save_changes_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            bool is_error = false;

            if (string.IsNullOrEmpty(txt_name.Text))
            {
                err_name.SetError(txt_name, "Name is required!");

                is_error = true;
            }

            if (string.IsNullOrEmpty(txt_username.Text))
            {
                err_username.SetError(txt_username, "Username is required!");

                is_error = true;
            }

            if (!is_error)
            {
                is_loading(true);

                bool update_success = await update_account();

                if (!update_success)
                {
                    is_loading(false);

                    err_username.SetError(txt_username, "Username is already in use!");

                    txt_username.Focus();
                }
            }
        }

        private async Task<bool> update_account()
        {
            return await Task.Run(() =>
            {
                bool response = false;

                Database database = new Database();

                string sql = "SELECT id FROM users WHERE username = @username AND id != @user_id";

                var parameters = new Dictionary<string, object>
                {
                    { "username", txt_username.Text },
                    { "user_id", Session.Get("user_id") }
                };

                var is_username_exists = database.Query(sql, parameters);

                if (is_username_exists.Count <= 0)
                {
                    DateTime currentDateTime = DateTime.Now;

                    string password = !string.IsNullOrEmpty(txt_password.Text) ? BCrypt.Net.BCrypt.HashPassword(txt_password.Text) : txt_old_password.Text;

                    var data = new Dictionary<string, object>
                    {
                        { "name", txt_name.Text },
                        { "username", txt_username.Text },
                        { "password", password },
                        { "updated_at", currentDateTime }
                    };

                    var conditions = new Dictionary<string, object>
                    {
                        { "id", Session.Get("user_id") }
                    };

                    database.Update("users", data, conditions);

                    Session.Set("user_name", txt_name.Text);
                    Session.Set("user_username", txt_username.Text);
                    Session.Set("user_password", password);

                    Invoke((MethodInvoker)delegate
                    {
                        Main main = Owner as Main;

                        main.display_data();

                        Hide();

                        MessageBox.Show("The account details have been updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Close();
                    });

                    response = true;
                }

                return response;
            });
        }

        private void Account_Settings_Load(object sender, EventArgs e)
        {
            txt_name.Text = Session.Get("user_name");
            txt_username.Text = Session.Get("user_username");
            txt_old_password.Text = Session.Get("user_password");
        }

        private void is_loading(bool state)
        {
            if (state)
            {
                is_busy = true;

                body.Visible = false;

                btn_save_changes.Enabled = false;
            }

            else
            {
                is_busy = false;

                body.Visible = true;

                btn_save_changes.Enabled = true;
            }
        }

        private void txt_name_KeyDown(object sender, KeyEventArgs e)
        {
            err_name.Dispose();
        }

        private void txt_username_KeyDown(object sender, KeyEventArgs e)
        {
            err_username.Dispose();
        }
    }
}