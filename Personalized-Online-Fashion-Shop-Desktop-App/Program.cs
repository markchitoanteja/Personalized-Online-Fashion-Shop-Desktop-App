using System;
using System.Windows.Forms;

namespace Personalized_Online_Fashion_Shop_Desktop_App
{
    internal static class Program
    {
        [STAThread]
        
        static void Main()
        {
            Database database = new Database();

            database.Initialize_Database();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
