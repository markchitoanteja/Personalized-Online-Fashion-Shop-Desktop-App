using Personalized_Online_Fashion_Shop_Desktop_App.ILLink;
using System;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class UC_Processed_Orders : UserControl
    {
        public UC_Processed_Orders()
        {
            InitializeComponent();
        }

        private void lv_orders_Resize(object sender, System.EventArgs e)
        {
            lv_orders.Columns[0].Width = 0;

            int availableWidth = lv_orders.ClientSize.Width;

            int column1Width = (int)(availableWidth * 0.15);
            int column2Width = (int)(availableWidth * 0.1);
            int column3Width = (int)(availableWidth * 0.1);
            int column4Width = (int)(availableWidth * 0.1);
            int column5Width = (int)(availableWidth * 0.40);
            int column6Width = (int)(availableWidth * 0.15);

            lv_orders.Columns[1].Width = column1Width;
            lv_orders.Columns[2].Width = column2Width;
            lv_orders.Columns[3].Width = column3Width;
            lv_orders.Columns[4].Width = column4Width;
            lv_orders.Columns[5].Width = column5Width;
            lv_orders.Columns[6].Width = column6Width;
        }

        private void lv_orders_Click(object sender, System.EventArgs e)
        {
            string tracking_number = lv_orders.SelectedItems[0].SubItems[1].Text;

            Process_Order process_order = new Process_Order(null, this);

            process_order.is_print = true;

            process_order.txt_discount.ReadOnly = true;
            process_order.txt_shipping_fee.ReadOnly = true;

            process_order.txt_discount.Text = lv_orders.SelectedItems[0].SubItems[2].Text.Replace("₱", "");
            process_order.txt_shipping_fee.Text = lv_orders.SelectedItems[0].SubItems[3].Text.Replace("₱", "");
            process_order.txt_total_price.Text = lv_orders.SelectedItems[0].SubItems[4].Text.Replace("₱", "");
            process_order.lbl_tracking_number.Text = tracking_number;
            process_order.lbl_title.Text = "Print Receipt";

            process_order.ShowDialog();
        }

        private void btn_update_order_status_Click(object sender, System.EventArgs e)
        {
            Update_Status update_status = new Update_Status(this);

            update_status.ShowDialog();
        }
    
        public void display_data()
        {
            Database database = new Database();

            string sql_2 = "SELECT * FROM shipped_orders ORDER BY id DESC";

            var shipped_orders = database.Query(sql_2);

            lv_orders.Items.Clear();

            foreach (var order in shipped_orders)
            {
                ListViewItem item = new ListViewItem(order["id"].ToString());

                item.SubItems.Add(order["tracking_number"].ToString());
                item.SubItems.Add("₱" + Convert.ToDecimal(order["discount"]).ToString("F2"));
                item.SubItems.Add("₱" + Convert.ToDecimal(order["shipping_fee"]).ToString("F2"));
                item.SubItems.Add("₱" + Convert.ToDecimal(order["total_price"]).ToString("F2"));
                item.SubItems.Add(order["description"].ToString());
                item.SubItems.Add(order["status"].ToString());

                lv_orders.Items.Add(item);
            }
        }
    }
}
