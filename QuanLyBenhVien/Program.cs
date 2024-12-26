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
            while (true)
            {
                using (SignIn signIn = new SignIn())
                {
                    if (signIn.ShowDialog() == DialogResult.OK)
                    {
                        // If login successful, run the MainForm
                        MainForm mainForm = new MainForm(signIn.user);
                        Application.Run(mainForm);

                        // If MainForm closes and Logout is triggered, loop back to LoginForm
                        if (mainForm.LogoutTriggered)
                        {
                            continue;
                        }
                    }
                }

                // Exit the loop and application if login fails or user cancels
                break;
            }
        }
    }
}
