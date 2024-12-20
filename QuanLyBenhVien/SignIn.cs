using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class SignIn : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public SignIn()
        {
            InitializeComponent();
        }

        private void AuthenticateByStaffID()
        {
            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter both StaffID and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = $"SELECT UserID, Pass, TypeOfStaff FROM USERLOGIN ul JOIN STAFF ON ul.UserID = StaffID WHERE UserID = '{txtUser.Text}'";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader != null)
                            {
                                if (reader.GetString(reader.GetOrdinal("Pass")) != txtPassword.Text)
                                {
                                    MessageBox.Show("Password isn't correct!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                else
                                {
                                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    string type = reader.GetString(reader.GetOrdinal("TypeOfStaff"));

                                    if (type.ToLower().Contains("Bác sĩ".ToLower()))
                                    {
                                        DoctorView doctorView = new DoctorView(txtUser.Text);
                                        doctorView.Show();
                                    }
                                }

                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy UserID", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            AuthenticateByStaffID();
        }
    }
}
