using System.Drawing;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class Main : Form
    {
        private bool is_exiting = false;
        private bool is_logout = false;
        private bool is_sidebar_collapsed = false;
        private string selected_sidebar_button;

        public Main()
        {
            InitializeComponent();
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
        }

        private void btn_manage_orders_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();
        }
    }
}
