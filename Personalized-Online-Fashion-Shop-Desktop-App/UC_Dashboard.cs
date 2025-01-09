using System;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    public partial class UC_Dashboard : UserControl
    {
        public UC_Dashboard()
        {
            InitializeComponent();
        }

        private void CenterControlHorizontally(Control control)
        {
            if (control?.Parent != null)
            {
                control.Left = (control.Parent.ClientSize.Width - control.Width) / 2;
            }
        }

        private void AdjustCardsWithPadding(Control parent, Control[] cards, int minWidth, int spacing, int edgePadding)
        {
            if (parent == null || cards == null || cards.Length == 0) return;

            int totalSpacing = spacing * (cards.Length - 1);
            int totalPadding = edgePadding * 2;
            int availableWidth = parent.ClientSize.Width - totalPadding - totalSpacing;
            int cardWidth = Math.Max(availableWidth / cards.Length, minWidth);
            int currentX = edgePadding;
            
            foreach (var card in cards)
            {
                card.Width = cardWidth;
                card.Left = currentX;
                currentX += cardWidth + spacing;
            }
        }

        private void ResizeListViewColumns(ListView listView)
        {
            if (listView.Columns.Count < 3) return;

            listView.Columns[0].Width = 0;

            int availableWidth = listView.ClientSize.Width;

            int column1Width = (int)(availableWidth * 0.3);
            int column2Width = (int)(availableWidth * 0.7);

            listView.Columns[1].Width = column1Width;
            listView.Columns[2].Width = column2Width;
        }

        private void UC_Dashboard_Resize(object sender, EventArgs e)
        {
            Control[] cards = { card_total_sales, card_active_customers, card_total_items };

            AdjustCardsWithPadding(this, cards, 230, 10, 20);

            CenterControlHorizontally(label4);
            CenterControlHorizontally(label5);
            CenterControlHorizontally(label8);
            ResizeListViewColumns(lv_recent_activities);
        }
    }
}
