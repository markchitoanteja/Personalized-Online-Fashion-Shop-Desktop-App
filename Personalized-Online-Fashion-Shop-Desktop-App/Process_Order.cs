using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using QRCoder;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class Process_Order : Form
    {
        private UC_Manage_Orders _uc_manage_orders;
        private UC_Processed_Orders _uc_processed_orders;

        private PrintDocument printDocument = new PrintDocument();
        private Bitmap panelBitmap;

        private float total_price = 0;
        private float gross_price = 0;
        private bool is_busy = false;
        private string[] selected_ids;

        public bool is_print = false;

        private static readonly Random Random = new Random();

        public Process_Order(UC_Manage_Orders uc_manage_orders = null, UC_Processed_Orders uc_processed_orders = null)
        {
            InitializeComponent();

            _uc_manage_orders = uc_manage_orders;
            _uc_processed_orders = uc_processed_orders;

            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void CenterControl(Control control)
        {
            if (control?.Parent != null)
            {
                control.Left = (control.Parent.ClientSize.Width - control.Width) / 2;
            }
        }

        private static string generate_tracking_number()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var trackingNumber = new StringBuilder(10);

            for (int i = 0; i < 10; i++)
            {
                trackingNumber.Append(chars[Random.Next(chars.Length)]);
            }

            return trackingNumber.ToString();
        }

        private void generate_qr_code(string data, PictureBox pictureBox)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    pictureBox.Image = qrCodeImage;
                }
            }
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

        private async void Process_Order_Load(object sender, EventArgs e)
        {
            btn_process_or_print.Text = is_print ? "&Print" : "&Process";
            btn_process_or_print.Enabled = false;

            is_busy = true;

            pnl_loading.BringToFront();
            pnl_loading.Visible = true;

            bool isDataLoaded = await display_data();

            if (isDataLoaded)
            {
                pnl_loading.Visible = false;

                is_busy = false;

                btn_process_or_print.Enabled = true;
            }
            else
            {
                Hide();

                MessageBox.Show("Failed to load data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                pnl_loading.Visible = false;

                is_busy = false;

                btn_process_or_print.Enabled = true;
            }
        }

        private async Task<bool> display_data()
        {
            return await Task.Run(() =>
            {
                try
                {
                    Database database = new Database();

                    if (!is_print)
                    {
                        float net_price = 0;

                        lbl_tracking_number.Invoke(new Action(() => { lbl_tracking_number.Text = generate_tracking_number(); }));

                        selected_ids = lbl_selected_ids.Text.Split(',');

                        Invoke(new Action(() => { lv_orders.Items.Clear(); }));

                        foreach (var id in selected_ids)
                        {
                            string sql = @"
                                SELECT 
                                    orders.id, 
                                    products.name AS product_name, 
                                    orders.quantity, 
                                    orders.total_price
                                FROM orders
                                INNER JOIN products ON orders.product_id = products.id
                                WHERE orders.id = @order_id";

                            var parameters = new Dictionary<string, object>
                            {
                                { "order_id", id }
                            };

                            var order = database.Query(sql, parameters);

                            if (order.Count > 0)
                            {
                                string itemVerb = int.Parse(order[0]["quantity"].ToString()) > 1 ? "Items" : "Item";

                                ListViewItem item = new ListViewItem(order[0]["id"].ToString());

                                item.SubItems.Add(order[0]["product_name"].ToString());
                                item.SubItems.Add(order[0]["quantity"].ToString() + " " + itemVerb);
                                item.SubItems.Add("₱" + Convert.ToDecimal(order[0]["total_price"]).ToString("F2"));

                                lv_orders.Invoke(new Action(() => { lv_orders.Items.Add(item); }));

                                net_price += float.Parse(order[0]["total_price"].ToString());
                            }
                        }

                        decimal finalNetPrice = Convert.ToDecimal(net_price);

                        txt_net_price.Invoke(new Action(() =>
                        {
                            txt_net_price.Text = finalNetPrice.ToString("F2");
                            txt_total_price.Text = finalNetPrice.ToString("F2");
                        }));

                        gross_price = net_price;
                    }
                    else
                    {
                        float gross_price = 0;
                        string sql = @"
                            SELECT 
                                orders.*, 
                                products.name AS product_name,
                                users.name AS user_name,
                                customers.address AS customer_address
                            FROM orders
                            INNER JOIN products ON orders.product_id = products.id
                            INNER JOIN users ON orders.user_id = users.id
                            INNER JOIN customers ON orders.user_id = customers.user_id
                            WHERE orders.tracking_number = @tracking_number";

                        var parameters = new Dictionary<string, object>
                        {
                            { "tracking_number", lbl_tracking_number.Text }
                        };

                        var orders = database.Query(sql, parameters);

                        lv_orders.Items.Clear();

                        string customer_name = orders[0]["user_name"].ToString();
                        string customer_address = orders[0]["customer_address"].ToString();

                        foreach (var order in orders)
                        {
                            string itemVerb = int.Parse(order["quantity"].ToString()) > 1 ? "Items" : "Item";

                            ListViewItem item = new ListViewItem(order["id"].ToString());

                            item.SubItems.Add(order["product_name"].ToString());
                            item.SubItems.Add(order["quantity"].ToString() + " " + itemVerb);
                            item.SubItems.Add("₱" + Convert.ToDecimal(order["total_price"]).ToString("F2"));

                            gross_price = gross_price + float.Parse(order["total_price"].ToString());

                            Invoke(new Action(() => { lv_orders.Items.Add(item); }));
                        }

                        Invoke(new Action(() => { lbl_customer_name.Text = customer_name; }));
                        Invoke(new Action(() => { lbl_complete_address.Text = customer_address; }));
                        Invoke(new Action(() => { txt_net_price.Text = Convert.ToDecimal(gross_price).ToString("F2"); }));
                    }

                    generate_qr_code(lbl_tracking_number.Text, pctrbx_qr_code);

                    Invoke(new Action(() => { CenterControl(lbl_customer_name); }));
                    Invoke(new Action(() => { CenterControl(lbl_tracking_number); }));

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            });
        }

        private void txt_discount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txt_shipping_fee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txt_discount_TextChanged(object sender, EventArgs e)
        {
            float discount = 0;
            float shipping_fee = 0;

            if (txt_discount.Text != "")
            {
                discount = float.Parse(txt_discount.Text);
            }

            if (txt_shipping_fee.Text != "")
            {
                shipping_fee = float.Parse(txt_shipping_fee.Text);
            }

            total_price = gross_price + shipping_fee - discount;

            txt_total_price.Text = Convert.ToDecimal(total_price).ToString("F2");
        }

        private async void btn_process_or_print_Click(object sender, EventArgs e)
        {
            if (!is_busy)
            {
                if (!is_print)
                {
                    is_busy = true;

                    pnl_loading.Visible = true;
                    btn_process_or_print.Enabled = false;

                    await process_data();

                    Hide();

                    MessageBox.Show("The orders have been successfully processed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close();
                }
                else
                {
                    CapturePanel(pnl_print_area);

                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
                    {
                        Document = printDocument
                    };

                    printPreviewDialog.ShowDialog();
                }
            }
        }

        private async Task<bool> process_data()
        {
            return await Task.Run(() =>
            {
                Database database = new Database();

                foreach (var id in selected_ids)
                {
                    string sql = "UPDATE orders SET tracking_number=@new_tracking_number, status='Processed' WHERE id=@order_id";

                    var parameters = new Dictionary<string, object>
                    {
                        { "new_tracking_number", lbl_tracking_number.Text },
                        { "order_id", id }
                    };

                    database.Query(sql, parameters);
                }

                string sql_2 = "INSERT INTO shipped_orders (uuid, tracking_number, discount, shipping_fee, total_price, description, status, created_at, updated_at) VALUES (@uuid, @tracking_number, @discount, @shipping_fee, @total_price, @description, @status, @created_at, @updated_at)";

                var parameters_2 = new Dictionary<string, object>
                {
                    { "uuid", database.Generate_UUID() },
                    { "tracking_number", lbl_tracking_number.Text },
                    { "discount", txt_discount.Text },
                    { "shipping_fee", txt_shipping_fee.Text },
                    { "total_price", txt_total_price.Text },
                    { "description", "Seller is preparing to ship your parcel." },
                    { "status", "Processed" },
                    { "created_at", DateTime.Now },
                    { "updated_at", DateTime.Now }
                };

                database.Query(sql_2, parameters_2);

                string sql_3 = "INSERT INTO track_orders (uuid, tracking_number, description, status, created_at, updated_at) VALUES (@uuid, @tracking_number, @description, @status, @created_at, @updated_at)";

                var parameters_3 = new Dictionary<string, object>
                {
                    { "uuid", database.Generate_UUID() },
                    { "tracking_number", lbl_tracking_number.Text },
                    { "description", "Seller is preparing to ship your parcel." },
                    { "status", "Processed" },
                    { "created_at", DateTime.Now },
                    { "updated_at", DateTime.Now }
                };

                database.Query(sql_3, parameters_3);

                _uc_manage_orders.Invoke(new Action(() => { _uc_manage_orders.display_data(); }));

                return true;
            });
        }

        private void lv_orders_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();

            e.Graphics.DrawString(e.Header.Text, lv_orders.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

            using (Pen gridPen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawLine(gridPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }
        }

        private void lv_orders_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawBackground();

            e.Graphics.DrawString(e.SubItem.Text, lv_orders.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);

            using (Pen gridPen = new Pen(Color.DarkGray, 1))
            {
                e.Graphics.DrawLine(gridPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
                e.Graphics.DrawLine(gridPen, e.Bounds.Right - 1, e.Bounds.Top, e.Bounds.Right - 1, e.Bounds.Bottom);
            }
        }

        private void CapturePanel(Panel panel)
        {
            panelBitmap = new Bitmap(panel.Width, panel.Height);

            panel.DrawToBitmap(panelBitmap, new Rectangle(0, 0, panel.Width, panel.Height));
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (panelBitmap != null)
            {
                int x = e.MarginBounds.Left;
                int y = e.MarginBounds.Top + 50;
                int width = e.MarginBounds.Width;
                int height = (panelBitmap.Height * width) / panelBitmap.Width;

                using (Pen borderPen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawRectangle(borderPen, e.MarginBounds);
                }

                string headerText = "Official Receipt";
                Font headerFont = new Font("Arial", 18, FontStyle.Bold);
                SizeF headerSize = e.Graphics.MeasureString(headerText, headerFont);
                e.Graphics.DrawString(headerText, headerFont, Brushes.Black, new PointF(e.MarginBounds.Left + (e.MarginBounds.Width - headerSize.Width) / 2, e.MarginBounds.Top - headerSize.Height - 10));

                string subHeaderText = "Thank you for your purchase!";
                Font subHeaderFont = new Font("Arial", 12, FontStyle.Regular);
                SizeF subHeaderSize = e.Graphics.MeasureString(subHeaderText, subHeaderFont);
                e.Graphics.DrawString(subHeaderText, subHeaderFont, Brushes.Gray, new PointF(e.MarginBounds.Left + (e.MarginBounds.Width - subHeaderSize.Width) / 2, e.MarginBounds.Top - 2 * headerSize.Height));

                string footerText = "Page 1 | Printed on " + DateTime.Now.ToString("MMMM dd, yyyy");
                Font footerFont = new Font("Arial", 10, FontStyle.Italic);
                SizeF footerSize = e.Graphics.MeasureString(footerText, footerFont);
                e.Graphics.DrawString(footerText, footerFont, Brushes.Gray, new PointF(e.MarginBounds.Left + (e.MarginBounds.Width - footerSize.Width) / 2, e.MarginBounds.Bottom + 10));

                using (Pen dividerPen = new Pen(Color.Black, 1))
                {
                    e.Graphics.DrawLine(dividerPen, e.MarginBounds.Left, e.MarginBounds.Top - 5, e.MarginBounds.Right, e.MarginBounds.Top - 5);

                    e.Graphics.DrawLine(dividerPen, e.MarginBounds.Left, e.MarginBounds.Bottom - 5, e.MarginBounds.Right, e.MarginBounds.Bottom - 5);
                }

                using (Brush backgroundBrush = new LinearGradientBrush(e.MarginBounds, Color.LightBlue, Color.White, LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.MarginBounds);
                }

                e.Graphics.DrawImage(panelBitmap, new Rectangle(x, y, width, height));

                string watermarkText = "DELIVERY";

                Font watermarkFont = new Font("Arial", 48, FontStyle.Bold);
                SizeF watermarkSize = e.Graphics.MeasureString(watermarkText, watermarkFont);
                using (Brush watermarkBrush = new SolidBrush(Color.FromArgb(50, Color.Gray)))
                {
                    e.Graphics.TranslateTransform(e.PageBounds.Width / 2, e.PageBounds.Height / 2);
                    e.Graphics.RotateTransform(-45);
                    e.Graphics.DrawString(watermarkText, watermarkFont, watermarkBrush, new PointF(-watermarkSize.Width / 2, -watermarkSize.Height / 2));
                    e.Graphics.ResetTransform();
                }
            }
        }
    }
}
