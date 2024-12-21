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
