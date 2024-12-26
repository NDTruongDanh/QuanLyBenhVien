using System;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool keepRunning = true;
            while (keepRunning)
            {
                using (SignIn signIn = new SignIn())
                {
                    DialogResult result = signIn.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            Form formToRun = null;

                            if (signIn.UserID == "admin")
                            {
                                formToRun = new MainForm(signIn.UserID);
                            }
                            else
                            { 

                                // Handle string comparison ignoring case
                                string userType = signIn.UserType?.Trim().ToLower();

                                switch (userType)
                                {
                                    case var type when type.Contains("bác sĩ"):
                                        formToRun = new DoctorView(signIn.UserID);
                                        break;
                                    case var type when type.Contains("dược sĩ"):
                                        formToRun = new PharmacistView(signIn.UserID, signIn.IsHeadDepartment);
                                        break;
                                    case var type when type.Contains("kế toán"):
                                        formToRun = new Accountant(signIn.UserID);
                                        break;
                                    case var type when type.Contains("y tá"):
                                        formToRun = new NurseVIew(signIn.UserID);
                                        break;
                                    default:
                                        MessageBox.Show($"Unknown user type: '{signIn.UserType}'");
                                        break;
                                }
                            }

                            if (formToRun != null)
                            {
                                Application.Run(formToRun);
                            }
                            else
                            {
                                MessageBox.Show("Could not create appropriate form for user type: " + signIn.UserType);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error creating form: {ex.Message}\nStack Trace: {ex.StackTrace}");
                        }
                    }
                    else
                    {
                        keepRunning = false;
                    }
                }
            }
        }
    }
}