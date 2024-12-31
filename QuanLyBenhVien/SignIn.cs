using QuanLyBenhVien.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace QuanLyBenhVien
{
    public partial class SignIn : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        public string UserID { get; private set; }
        public string UserType { get; private set; }
        public bool IsHeadDepartment { get; private set; }

        public SignIn()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
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

                    string staffSql = @"SELECT UserID, Pass, TypeOfStaff, HeadDepartmentID 
                                  FROM USERLOGIN ul 
                                  JOIN STAFF st ON ul.UserID = st.StaffID 
                                  JOIN DEPARTMENT d ON d.DepartmentID = st.DepartmentID 
                                  WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(staffSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", txtUser.Text);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetString(reader.GetOrdinal("Pass")) == txtPassword.Text)
                                {
                                    UserID = txtUser.Text;
                                    UserType = reader.GetString(reader.GetOrdinal("TypeOfStaff"));
                                    IsHeadDepartment = (txtUser.Text == reader.GetString(reader.GetOrdinal("HeadDepartmentID")));

                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Password isn't correct!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("UserID not found", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (chkRememberMe.Checked)
            {
                if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin UserID và Password.");
                    return;
                }

                string query = "UPDATE USERLOGIN SET FLAG = @Flag WHERE UserID = @UserID AND Pass = @Password";
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Flag", "1");
                            command.Parameters.AddWithValue("@UserID", txtUser.Text);
                            command.Parameters.AddWithValue("@Password", txtPassword.Text);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    }
                }
            }

            AuthenticateByStaffID();
            txtPassword.Clear();
            txtUser.Clear();
            chkRememberMe.Checked = false;
        }

        

        private void btnHidePass_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == true)
            {

                txtPassword.UseSystemPasswordChar = false;
                btnHidePass.BackgroundImage = Properties.Resources.an;
                btnHidePass.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {

                txtPassword.UseSystemPasswordChar = true;
                btnHidePass.BackgroundImage = Properties.Resources.mo;
                btnHidePass.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            string query1 = "SELECT Pass FROM USERLOGIN WHERE UserID = @UserID";

           

            // Lấy giá trị từ TextBox nhập tên tài khoản
            string username = txtUser.Text.Trim();

            // Biến lưu mật khẩu
            string password = "";

            // Kết nối và thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                
                    connection.Open(); // Mở kết nối
                    using (SqlCommand command = new SqlCommand(query1, connection))
                    {
                        // Gắn giá trị tham số @Username
                        command.Parameters.AddWithValue("@UserID", username);

                        // Thực hiện truy vấn
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Lấy giá trị cột Password
                                password = reader["Pass"].ToString();
                            }
                            
                        }
                    }
            }

            string query2 = "SELECT FLAG FROM USERLOGIN WHERE UserID = @UserID";

            // Biến để lưu kết quả từ database
            string result = "";

            // Kết nối và thực hiện truy vấn
            using (SqlConnection connection = new SqlConnection(connStr))
            {
              
                    connection.Open(); // Mở kết nối

                using (SqlCommand command = new SqlCommand(query2, connection))
                {
                    // Thêm tham số vào câu lệnh SQL
                    command.Parameters.AddWithValue("@UserID", username);

                    // Thực hiện truy vấn
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Lấy giá trị cột Password
                            result = reader["FLAG"].ToString();
                        }

                    }

                }
            }

            if (result != "0")
            {
                txtPassword.Text = password;
            }
        }
    }
    public interface ILogoutHandler
    {
        bool LogoutTriggered { get; }
    }
}
