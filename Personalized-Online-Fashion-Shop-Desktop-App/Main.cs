using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class Main : Form
    {
        private bool is_busy = false;
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

        private void reset_sidebar_buttons()
        {
            btn_dashboard.BackColor = Color.Transparent;
            btn_manage_orders.BackColor = Color.Transparent;
            btn_processed_orders.BackColor = Color.Transparent;
            btn_logout.BackColor = Color.Transparent;
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

        private async void mouse_click(Control control)
        {
            is_busy = true;

            reset_sidebar_buttons();

            control.BackColor = Color.FromArgb(13, 110, 253);

            selected_sidebar_button = control.Name;

            User_Control_Loading.BringToFront();

            switch (selected_sidebar_button)
            {
                case "btn_dashboard":
                    await display_data("display_dashboard_data");

                    User_Control_Dashboard.Invoke(new Action(() => User_Control_Dashboard.BringToFront()));

                    User_Control_Dashboard.BringToFront();

                    is_busy = false;

                    break;

                case "btn_manage_orders":
                    await display_data("display_orders");

                    User_Control_Manage_Orders.Invoke(new Action(() => User_Control_Manage_Orders.BringToFront()));

                    User_Control_Manage_Orders.BringToFront();

                    is_busy = false;

                    break;

                case "btn_processed_orders":
                    await display_data("display_processed_orders");

                    User_Control_Processed_Orders.Invoke(new Action(() => User_Control_Processed_Orders.BringToFront()));

                    User_Control_Processed_Orders.BringToFront();

                    is_busy = false;

                    break;

                default:
                    break;
            }
        }

        private string generate_order_id(int orderId)
        {
            return $"#{orderId:D5}";
        }

        private async Task<bool> display_data(string data)
        {
            return await Task.Run(() =>
            {
                Database database = new Database();

                switch (data)
                {
                    case "display_dashboard_data":
                        var result = database.Query(@"
                            SELECT 
                                COALESCE((SELECT SUM(total_price) FROM orders WHERE status = 'Completed'), 0) AS total_sales,
                                (SELECT COUNT(id) FROM users) AS active_users,
                                (SELECT COUNT(id) FROM products) AS total_items,
                                l.id AS log_id, 
                                l.created_at AS log_created_at, 
                                l.activity AS log_activity
                            FROM 
                                logs l
                            ORDER BY 
                                l.created_at DESC
                            LIMIT 5
                        ");

                        if (result.Count > 0)
                        {
                            float total_sales = result[0]["total_sales"] != DBNull.Value ? Convert.ToSingle(result[0]["total_sales"]) : 0;
                            int active_users = Convert.ToInt32(result[0]["active_users"]);
                            int total_items = Convert.ToInt32(result[0]["total_items"]);

                            User_Control_Dashboard.Invoke(new Action(() =>
                            {
                                User_Control_Dashboard.lbl_total_sales.Text = "₱" + total_sales.ToString("F2");
                                User_Control_Dashboard.lbl_active_users.Text = active_users.ToString();
                                User_Control_Dashboard.lbl_total_items.Text = total_items.ToString();
                            }));

                            User_Control_Dashboard.lv_recent_activities.Invoke(new Action(() =>
                            {
                                User_Control_Dashboard.lv_recent_activities.Items.Clear();

                                foreach (var row in result)
                                {
                                    var log_id = row["log_id"].ToString();
                                    var log_created_at = Convert.ToDateTime(row["log_created_at"]).ToString("MMMM dd, yyyy hh:mm tt");
                                    var log_activity = row["log_activity"].ToString();

                                    ListViewItem item = new ListViewItem(log_id);
                                    item.SubItems.Add(log_created_at);
                                    item.SubItems.Add(log_activity);

                                    User_Control_Dashboard.lv_recent_activities.Items.Add(item);
                                }
                            }));
                        }

                        break;

                    case "display_orders":
                        string sql = "SELECT orders.*, users.name AS user_name, products.name AS product_name FROM orders JOIN users ON orders.user_id = users.id JOIN products ON orders.product_id = products.id WHERE orders.status != @status_cart AND orders.status != @status_placed AND orders.status != @status_cancelled ORDER BY orders.user_id ASC, orders.id DESC";

                        var parameters = new Dictionary<string, object>
                        {
                            { "status_cart", "Cart" },
                            { "status_placed", "Placed" },
                            { "status_cancelled", "Cancelled" }
                        };

                        var orders = database.Query(sql, parameters);

                        User_Control_Manage_Orders.Invoke(new Action(() =>
                        {
                            User_Control_Manage_Orders.lv_orders.Items.Clear();

                            foreach (var order in orders)
                            {
                                string item_verb = int.Parse(order["quantity"].ToString()) > 1 ? "Items" : "Item";

                                ListViewItem item = new ListViewItem(order["id"].ToString());

                                item.SubItems.Add(generate_order_id(int.Parse(order["id"].ToString())));
                                item.SubItems.Add(order["user_name"].ToString());
                                item.SubItems.Add(order["product_name"].ToString());
                                item.SubItems.Add(order["quantity"].ToString() + " " + item_verb);
                                item.SubItems.Add("₱" + Convert.ToDecimal(order["total_price"]).ToString("F2"));
                                item.SubItems.Add(order["status"].ToString());

                                User_Control_Manage_Orders.lv_orders.Items.Add(item);
                            }

                            User_Control_Manage_Orders.lbl_selected_ids.Text = "";
                            User_Control_Manage_Orders.btn_process_order.Visible = false;
                        }));

                        break;

                    case "display_processed_orders":
                        string sql_2 = "SELECT * FROM shipped_orders ORDER BY id DESC";

                        var shipped_orders = database.Query(sql_2);

                        User_Control_Processed_Orders.Invoke(new Action(() =>
                        {
                            User_Control_Processed_Orders.lv_orders.Items.Clear();

                            foreach (var order in shipped_orders)
                            {
                                ListViewItem item = new ListViewItem(order["id"].ToString());

                                item.SubItems.Add(order["tracking_number"].ToString());
                                item.SubItems.Add("₱" + Convert.ToDecimal(order["discount"]).ToString("F2"));
                                item.SubItems.Add("₱" + Convert.ToDecimal(order["shipping_fee"]).ToString("F2"));
                                item.SubItems.Add("₱" + Convert.ToDecimal(order["total_price"]).ToString("F2"));
                                item.SubItems.Add(order["description"].ToString());
                                item.SubItems.Add(order["status"].ToString());

                                User_Control_Processed_Orders.lv_orders.Items.Add(item);
                            }
                        }));

                        break;

                    default:
                        break;
                }

                return true;
            });
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!is_busy)
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
            else
            {
                e.Cancel = true;
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

            btn_dashboard.PerformClick();
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
            mouse_enter(btn_processed_orders);
        }

        private void btn_logout_MouseLeave(object sender, System.EventArgs e)
        {
            mouse_leave(btn_processed_orders);
        }

        private void btn_dashboard_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            if (!is_busy)
            {
                mouse_click(btn_dashboard);
            }
        }

        private void btn_manage_orders_Click(object sender, System.EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            if (!is_busy)
            {
                mouse_click(btn_manage_orders);
            }
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

        private void btn_account_settings_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            Account_Settings account_settings = new Account_Settings();

            account_settings.Owner = this;
            account_settings.ShowDialog();
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            About about = new About();

            about.ShowDialog();
        }

        private void btn_logout_2_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            btn_logout.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            if (!is_busy)
            {
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
        }

        private void btn_logout_MouseEnter_1(object sender, EventArgs e)
        {
            mouse_enter(btn_logout);
        }

        private void btn_logout_MouseLeave_1(object sender, EventArgs e)
        {
            mouse_leave(btn_logout);
        }

        private void btn_processed_orders_Click(object sender, EventArgs e)
        {
            btn_temp.Focus();

            hide_dropdown();

            if (!is_busy)
            {
                mouse_click(btn_processed_orders);
            }
        }
    }
}
