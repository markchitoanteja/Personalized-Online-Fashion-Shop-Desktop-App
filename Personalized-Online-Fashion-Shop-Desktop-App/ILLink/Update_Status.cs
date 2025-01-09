using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Personalized_Online_Fashion_Shop_Desktop_App.ILLink
{
    public partial class Update_Status : Form
    {
        private bool is_busy = false;
        private bool is_update_ready = false;

        private UC_Processed_Orders _uc_processed_orders;

        public Update_Status(UC_Processed_Orders uc_processed_orders)
        {
            InitializeComponent();
            _uc_processed_orders = uc_processed_orders;
        }

        private void AddFocusHandler(Control parent, bool activate)
        {
            foreach (Control control in parent.Controls)
            {
                if (control != txt_tracking_number)
                {
                    if (activate)
                    {
                        control.Enter += RedirectFocus;
                        control.MouseDown += RedirectFocus;
                    }
                    else
                    {
                        control.Enter -= RedirectFocus;
                        control.MouseDown -= RedirectFocus;
                    }
                }

                if (control.HasChildren)
                {
                    AddFocusHandler(control, activate);
                }
            }
        }

        private void RedirectFocus(object sender, EventArgs e)
        {
            txt_tracking_number.Focus();
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

        private void Update_Status_Load(object sender, EventArgs e)
        {
            AddFocusHandler(this, true);
        }

        private async void txt_tracking_number_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!String.IsNullOrEmpty(txt_tracking_number.Text) && !is_update_ready && !is_busy)
                {
                    is_busy = true;

                    pnl_loading.BringToFront();
                    pnl_loading.Visible = true;

                    if (await get_order_data())
                    {
                        Invoke(new Action(() =>
                        {
                            Size = new Size(500, 541);
                            AddFocusHandler(this, false);
                            btn_update_status.Enabled = true;
                            txt_tracking_number.ReadOnly = true;

                            var screen = Screen.FromControl(this).WorkingArea;
                            int x = (screen.Width - this.Width) / 2;
                            int y = (screen.Height - this.Height) / 2;

                            Location = new Point(x, y);
                        }));

                        is_update_ready = true;
                        is_busy = false;
                    }
                    else
                    {
                        Invoke(new Action(() => { txt_tracking_number.Clear(); }));

                        is_busy = false;
                    }
                }
            }
        }

        private async Task<bool> get_order_data()
        {
            return await Task.Run(() =>
            {
                Database database = new Database();

                string sql = @"
                    SELECT 
                        so.*, 
                        o.user_id, 
                        u.name AS customer_name, 
                        c.address AS customer_address
                    FROM 
                        shipped_orders so
                    JOIN 
                        orders o ON so.tracking_number = o.tracking_number
                    JOIN 
                        users u ON o.user_id = u.id
                    JOIN 
                        customers c ON o.user_id = c.user_id
                    WHERE 
                        so.tracking_number = @tracking_number AND so.status != 'Completed'";

                var parameters = new Dictionary<string, object>
                {
                    { "tracking_number", txt_tracking_number.Text }
                };

                var orders = database.Query(sql, parameters);

                if (orders.Count > 0)
                {
                    foreach (var order in orders)
                    {
                        Invoke(new Action(() =>
                        {
                            lbl_customer_name.Text = order["customer_name"].ToString();
                            lbl_complete_address.Text = order["customer_address"].ToString();
                            lbl_discount.Text = "₱" + Convert.ToDecimal(order["discount"]).ToString("F2");
                            lbl_shipping_fee.Text = "₱" + Convert.ToDecimal(order["shipping_fee"]).ToString("F2");
                            lbl_total_price.Text = "₱" + Convert.ToDecimal(order["total_price"]).ToString("F2");
                        }));
                    }

                    Invoke(new Action(() => { pnl_loading.Visible = false; }));

                    return true;
                }
                else
                {
                    Invoke(new Action(() => { pnl_loading.Visible = false; }));

                    MessageBox.Show("The tracking number you entered is invalid. Please check and try again.", "Invalid Tracking Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return false;
                }
            });
        }

        private async void btn_update_status_Click(object sender, EventArgs e)
        {
            if (!is_busy)
            {
                if (!String.IsNullOrEmpty(txt_description.Text) && txt_status.Text != null)
                {
                    btn_update_status.Enabled = false;

                    is_busy = true;

                    pnl_loading.BringToFront();
                    pnl_loading.Visible = true;

                    Size = new Size(500, 393);

                    var screen = Screen.FromControl(this).WorkingArea;
                    int x = (screen.Width - this.Width) / 2;
                    int y = (screen.Height - this.Height) / 2;

                    Location = new Point(x, y);

                    Database database = new Database();

                    if (await update_status(database.Generate_UUID(), txt_status.Text, txt_description.Text, txt_tracking_number.Text, DateTime.Now, DateTime.Now))
                    {
                        Invoke(new Action(() => { Hide(); }));

                        MessageBox.Show("Order status has been successfully updated.", "Updated Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Invoke(new Action(() => { Close(); }));
                    }
                }
            }
        }

        private async Task<bool> update_status(string uuid, string status, string description, string tracking_number, DateTime created_at, DateTime updated_at)
        {
            return await Task.Run(() =>
            {
                Database database = new Database();

                string sql = @"
                    START TRANSACTION;

                    UPDATE shipped_orders 
                    SET status = @status, description = @description 
                    WHERE tracking_number = @tracking_number;

                    UPDATE orders 
                    SET status = @status 
                    WHERE tracking_number = @tracking_number;

                    INSERT INTO track_orders (uuid, tracking_number, description, status, created_at, updated_at) 
                    VALUES (@uuid, @tracking_number, @description, @status, @created_at, @updated_at);

                    COMMIT;";


                var parameters = new Dictionary<string, object>
                {
                    { "uuid", uuid },
                    { "tracking_number", tracking_number },
                    { "description", description },
                    { "status", status },
                    { "created_at", created_at },
                    { "updated_at", updated_at }
                };

                database.Query(sql, parameters);

                Invoke(new Action(() => { _uc_processed_orders.display_data(); }));
                
                return true;
            });
        }
    }
}
