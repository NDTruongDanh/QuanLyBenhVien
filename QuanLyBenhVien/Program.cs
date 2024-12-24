using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (SignIn signIn = new SignIn())
            {
                if (signIn.ShowDialog() == DialogResult.OK) // Check if login was successful
                {
                    Application.Run(new MainForm(signIn.user));
                }
                else
                {
                    // Exit the application if login failed or was canceled
                    Application.Exit();
                }
            }
        }
    }
}
