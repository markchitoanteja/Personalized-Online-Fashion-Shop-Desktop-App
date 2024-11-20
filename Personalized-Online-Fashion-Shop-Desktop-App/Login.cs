using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class Login : Form
    {
        int counter;

        public Login()
        {
            InitializeComponent();
        }

        private void login_submit_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            bool is_error = false;

            if (string.IsNullOrEmpty(login_username.Text))
            {
                err_username.SetError(login_username, "Username is Required!");

                is_error = true;
            }

            if (string.IsNullOrEmpty(login_password.Text))
            {
                err_password.SetError(login_password, "Password is Required!");

                is_error = true;
            }

            if (!is_error)
            {
                counter = 0;

                is_loading(true);

                timer1.Start();
            }
        }

        private bool password_verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        private void is_loading(bool state)
        {
            if (state)
            {
                main_form.Visible = false;
                loading.Visible = true;
            }
            else
            {
                main_form.Visible = true;
                loading.Visible = false;
            }
        }

        private void login_username_KeyDown(object sender, KeyEventArgs e)
        {
            err_username.Dispose();
        }

        private void login_password_KeyDown(object sender, KeyEventArgs e)
        {
            err_password.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;

            if (counter == 3)
            {
                timer1.Stop();

                Database database = new Database();
                bool response = false;

                var user_data = database.Select_One("users", "username", login_username.Text);

                if (user_data.Count > 0)
                {
                    if (login_username.Text == user_data["username"].ToString())
                    {
                        string hash = user_data["password"].ToString();

                        if (password_verify(login_password.Text, hash))
                        {
                            response = true;
                        }
                    }
                }

                if (response)
                {
                    Hide();

                    Session.Set("user_id", user_data["id"].ToString());

                    if (login_remember_me.Checked)
                    {
                        Session.Set("username", login_username.Text);
                        Session.Set("password", login_password.Text);
                    }

                    else
                    {
                        Session.Remove("username");
                        Session.Remove("password");
                    }

                    MessageBox.Show("Welcome back, " + user_data["name"] + "!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Main main = new Main();

                    main.Show();
                }

                else
                {
                    is_loading(false);

                    MessageBox.Show("The username or password you entered is incorrect. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (Session.Exists("username") && Session.Exists("password"))
            {
                login_username.Text = Session.Get("username");
                login_password.Text = Session.Get("password");

                login_remember_me.Checked = true;
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
