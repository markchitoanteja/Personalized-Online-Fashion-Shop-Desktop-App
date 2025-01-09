using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class UC_Manage_Orders : UserControl
    {
        public UC_Manage_Orders()
        {
            InitializeComponent();
        }

        private void UC_Manage_Orders_Resize(object sender, EventArgs e)
        {
            lv_orders.Columns[0].Width = 0;

            int availableWidth = lv_orders.ClientSize.Width;

            int column1Width = (int)(availableWidth * 0.15);
            int column2Width = (int)(availableWidth * 0.25);
            int column3Width = (int)(availableWidth * 0.25);
            int column4Width = (int)(availableWidth * 0.1);
            int column5Width = (int)(availableWidth * 0.1);
            int column6Width = (int)(availableWidth * 0.15);

            lv_orders.Columns[1].Width = column1Width;
            lv_orders.Columns[2].Width = column2Width;
            lv_orders.Columns[3].Width = column3Width;
            lv_orders.Columns[4].Width = column4Width;
            lv_orders.Columns[5].Width = column5Width;
            lv_orders.Columns[6].Width = column6Width;
        }

        private void lv_orders_MouseDown(object sender, MouseEventArgs e)
        {
            if (lv_orders.HitTest(e.Location).Item == null)
            {
                lv_orders.SelectedItems.Clear();

                lbl_selected_ids.Text = "";

                btn_process_order.Visible = false;
            }
        }

        private void btn_process_order_Click(object sender, EventArgs e)
        {
            Process_Order process_order = new Process_Order(this, null);

            process_order.lbl_selected_ids.Text = lbl_selected_ids.Text;
            process_order.lbl_customer_name.Text = lbl_customer_name.Text;

            process_order.is_print = false;

            process_order.ShowDialog();
        }

        private string generate_order_id(int orderId)
        {
            return $"#{orderId:D5}";
        }

        public void display_data()
        {
            Database database = new Database();

            string sql = "SELECT orders.*, users.name AS user_name, products.name AS product_name FROM orders JOIN users ON orders.user_id = users.id JOIN products ON orders.product_id = products.id WHERE orders.status != @status_cart AND orders.status != @status_placed AND orders.status != @status_cancelled ORDER BY orders.id DESC";

            var parameters = new Dictionary<string, object>
            {
                { "status_cart", "Cart" },
                { "status_placed", "Placed" },
                { "status_cancelled", "Cancelled" }
            };

            var orders = database.Query(sql, parameters);

            lv_orders.Items.Clear();

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

                lv_orders.Items.Add(item);
            }

            lbl_selected_ids.Text = "";
            btn_process_order.Visible = false;
        }

        private void lv_orders_Click(object sender, EventArgs e)
        {
            if (lv_orders.SelectedItems.Count > 0)
            {
                if (lv_orders.SelectedItems.Count > 10)
                {
                    MessageBox.Show("You can select a maximum of 10 items per tracking number.", "Selection Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    lv_orders.SelectedItems.Clear();
                    lbl_selected_ids.Text = "";
                    btn_process_order.Visible = false;

                    return;
                }

                List<string> selectedIds = new List<string>();
                List<string> selectedUserIds = new List<string>();
                List<string> selectedStatuses = new List<string>();

                foreach (ListViewItem item in lv_orders.SelectedItems)
                {
                    selectedIds.Add(item.Text);
                    selectedUserIds.Add(item.SubItems[2].Text);
                    selectedStatuses.Add(item.SubItems[6].Text);

                    lbl_customer_name.Text = item.SubItems[2].Text;
                }

                if (selectedUserIds.Distinct().Count() > 1)
                {
                    MessageBox.Show("Selected orders belong to different users. Please select orders from the same user.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    lv_orders.SelectedItems.Clear();
                    lbl_selected_ids.Text = "";
                    btn_process_order.Visible = false;

                    return;
                }

                if (selectedStatuses.Any(status => status != "Approved"))
                {
                    MessageBox.Show("All selected orders must have 'Approved' status. Please check your selection.", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    lv_orders.SelectedItems.Clear();
                    lbl_selected_ids.Text = "";
                    btn_process_order.Visible = false;

                    return;
                }

                lbl_selected_ids.Text = string.Join(", ", selectedIds);
                btn_process_order.Visible = true;
            }
            else
            {
                lbl_selected_ids.Text = "";
                btn_process_order.Visible = false;
            }
        }
    }
}
