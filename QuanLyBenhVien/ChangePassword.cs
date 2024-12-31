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
    public partial class ChangePassword : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        string userID = null;

        string oldPw = null;

        DataTable dataTable = new DataTable();
        public ChangePassword(string userID)
        {
            InitializeComponent();
            this.userID = userID;
            LoadData();
            oldPw = dataTable.Rows[0]["Pass"].ToString();
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = $"SELECT * FROM USERLOGIN WHERE UserID = '{userID}'";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn))
                    {
                        dataAdapter.Fill(dataTable);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}!");
                }

            }
        }
        private void btnChangePass_Click(object sender, EventArgs e)
        {
            ChangePW();

          
        }

        private void ChangePW()
        {
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
                        command.Parameters.AddWithValue("@Flag", "0");
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@Password", txtOldPass.Text);

                        // Thực thi câu lệnh
                        int rowsAffected = command.ExecuteNonQuery();

                        // Kiểm tra kết quả
                      
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }

            //Hung Chinh
            if (txtOldPass.Text == oldPw)
            {
                if (txtNewPass.Text == txtConfirmPass.Text)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connStr))
                        {
                            conn.Open();
                            string sql = $@"UPDATE USERLOGIN
                                        SET Pass = '{txtNewPass.Text}'
                                        WHERE UserID = '{userID}'";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Đổi mật khẩu thành công!");
                                this.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("New passwords do not match!");
                }
            }
            else
            {
                MessageBox.Show("Old password is incorrect!");
            }
        }
    }
}
