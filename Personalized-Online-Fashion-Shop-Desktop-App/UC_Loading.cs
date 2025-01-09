using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class UC_Loading : UserControl
    {
        public UC_Loading()
        {
            InitializeComponent();
        }

        private void CenterControl(Control control)
        {
            if (control?.Parent != null)
            {
                control.Left = (control.Parent.ClientSize.Width - control.Width) / 2;
                control.Top = (control.Parent.ClientSize.Height - control.Height) / 2;
            }
        }

        private void UC_Loading_Resize(object sender, System.EventArgs e)
        {
            CenterControl(panel1);
        }
    }
}
