using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class Main : Form
    {
        private bool is_exiting = false;
        private bool is_logout = false;
        private bool is_sidebar_collapsed = false;
        private bool is_dropdown_visible = false;
        private string selected_sidebar_button;

        public Main()
        {
            InitializeComponent();
        }

        public void display_data()
        {
            lbl_user_name.Text = Session.Get("user_name");
        }

        private void mouse_enter(Control control)
        {
            if (selected_sidebar_button != control.Name)
            {
                control.BackColor = Color.DarkGray;
            }

            else
            {
                control.BackColor = Color.FromArgb(13, 110, 253);
            }
        }

        private void mouse_leave(Control control)
        {
            if (selected_sidebar_button != control.Name)
            {
                control.BackColor = Color.Transparent;
            }
        }

        private void mouse_click(Control control)
        {
            reset_sidebar_buttons();

            control.BackColor = Color.FromArgb(13, 110, 253);
            selected_sidebar_button = control.Name;

            switch (selected_sidebar_button)
            {
                case "btn_dashboard":
                    User_Control_Dashboard.BringToFront();

                    break;

                case "btn_manage_orders":
                    User_Control_Manage_Orders.BringToFront();

                    break;

                default:
                    break;
            }
        }

        private void reset_sidebar_buttons()
        {
            btn_dashboard.BackColor = Color.Transparent;
            btn_manage_orders.BackColor = Color.Transparent;
            btn_logout.BackColor = Color.Transparent;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!is_exiting && !is_logout)
            {
                DialogResult dialog_result = MessageBox.Show("Are you sure you want to close the application?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog_result == DialogResult.Yes)
                {
                    is_exiting = true;

                    Application.Exit();
                }

                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void btn_toogle_menu_Click(object sender, System.EventArgs e)
        {
            if (!is_sidebar_collapsed)
            {
                sidebar.Width = 50;

                is_sidebar_collapsed = true;
            }

            else
            {
                sidebar.Width = 250;

                is_sidebar_collapsed = false;
            }
        }

        private void Main_Load(object sender, System.EventArgs e)
        {
            display_data();

            selected_sidebar_button = "btn_dashboard";
        }

        private void btn_dashboard_MouseEnter(object sender, System.EventArgs e)
        {
            mouse_enter(btn_dashboard);
        }

        private void btn_dashboard_MouseLeave(object sender, System.EventArgs e)
        {
            mouse_leave(btn_dashboard);
        }

        private void btn_manage_orders_MouseEnter(object sender, System.EventArgs e)
        {
            mouse_enter(btn_manage_orders);
        }

        private void btn_manage_orders_MouseLeave(object sender, System.EventArgs e)
        {
            mouse_leave(btn_manage_orders);
        }

        private void btn_logout_MouseEnter(object sender, System.EventArgs e)
        {
            mouse_enter(btn_logout);
        }

        private void btn_logout_MouseLeave(object sender, System.EventArgs e)
        {
            mouse_leave(btn_logout);
        }

        private void btn_logout_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            DialogResult dialog_result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialog_result == DialogResult.Yes)
            {
                Session.Remove("user_id");

                is_logout = true;

                Login login = new Login();

                login.Show();

                Close();
            }
        }

        private void btn_dashboard_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            mouse_click(btn_dashboard);
        }

        private void btn_manage_orders_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            mouse_click(btn_manage_orders);
        }

        private void btn_settings_Click(object sender, System.EventArgs e)
        {
            if (!is_dropdown_visible)
            {
                dropdown_settings.BringToFront();

                dropdown_settings.Visible = true;

                is_dropdown_visible = true;
            }

            else
            {
                hide_dropdown();
            }
        }

        private void hide_dropdown()
        {
            dropdown_settings.Visible = false;

            is_dropdown_visible = false;
        }

        private void btn_account_settings_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            Account_Settings account_settings = new Account_Settings();

            account_settings.Owner = this;
            account_settings.ShowDialog();
        }

        private void btn_logout_2_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            btn_logout.PerformClick();
        }

        private void btn_developers_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            About about = new About();

            about.ShowDialog();
        }
    }
}
