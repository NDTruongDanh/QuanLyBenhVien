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

        public SignIn()
        {
            InitializeComponent();

            txtPassword.UseSystemPasswordChar = true;
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
                    string sql;
                    if (txtUser.Text != "admin")
                    {
                        sql = $@"SELECT UserID, Pass, TypeOfStaff, HeadDepartmentID 
                                   FROM USERLOGIN ul JOIN STAFF st ON ul.UserID = st.StaffID JOIN DEPARTMENT d ON d.DepartmentID = st.DepartmentID 
                                   WHERE UserID = '{txtUser.Text}'";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetString(reader.GetOrdinal("Pass")) != txtPassword.Text)
                                {
                                    MessageBox.Show("Password isn't correct!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                else
                                {
                                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    string type = reader.GetString(reader.GetOrdinal("TypeOfStaff"));

                                    bool isHeadDepartment = false;
                                    if (txtUser.Text == reader.GetString(reader.GetOrdinal("HeadDepartmentID")))
                                    {
                                        isHeadDepartment = true;
                                    }

                                    if (type.ToLower().Contains("Bác sĩ".ToLower()))
                                    {
                                        DoctorView doctorView = new DoctorView(txtUser.Text);
                                        doctorView.ShowDialog();
                                    }
                                    else if (type.ToLower().Contains("Dược sĩ".ToLower()))
                                    {
                                        PharmacistView pharmacistView = new PharmacistView(txtUser.Text, isHeadDepartment);
                                        pharmacistView.ShowDialog();
                                    }
                                    else if (type.ToLower().Contains("Kế toán".ToLower()))
                                    {
                                        Accountant accountant = new Accountant(txtUser.Text);
                                        accountant.ShowDialog();
                                    }
                                    else if (type.ToLower().Contains("Y tá".ToLower()))
                                    {
                                        NurseVIew nurseVIew = new NurseVIew(txtUser.Text);
                                        nurseVIew.ShowDialog();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy UserID", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        sql = $@"SELECT * FROM USERLOGIN
                                 WHERE UserID = '{txtUser.Text}'";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetString(reader.GetOrdinal("Pass")) != txtPassword.Text)
                                {
                                    MessageBox.Show("Password isn't correct!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    MainForm mainForm = new MainForm(txtUser.Text);
                                    mainForm.ShowDialog();
                                }
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
            // Kiểm tra nếu checkbox Remember Me được chọn
            if (chkRememberMe.Checked)
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin UserID và Password.");
                    return;
                }

                string query = "UPDATE USERLOGIN SET FLAG = @Flag WHERE UserID = @UserID AND Pass = @Password";

                // Sử dụng kết nối cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    try
                    {
                        connection.Open(); // Mở kết nối

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Thêm tham số cho câu lệnh
                            command.Parameters.AddWithValue("@Flag", "1");
                            command.Parameters.AddWithValue("@UserID", txtUser.Text);
                            command.Parameters.AddWithValue("@Password", txtPassword.Text);

                            // Thực thi câu lệnh
                            int rowsAffected = command.ExecuteNonQuery();

                            // Kiểm tra kết quả
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Đã lưu mật khẩu thành công");
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy người dùng phù hợp hoặc mật khẩu sai.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    }
                }
            }

            AuthenticateByStaffID();
            txtPassword.Clear();
            txtUser.Clear();
            chkRememberMe.Checked = false;
        }


        // AuthenticateByStaffID();
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

                    // Hiển thị kết quả trong Label hoặc TextBox
                   
                
              
            }

            if (result != "0")
            {
                txtPassword.Text = password;
            }
        }
    }
}
