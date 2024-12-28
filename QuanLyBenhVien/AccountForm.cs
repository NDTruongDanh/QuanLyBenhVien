using QuanLyBenhVien.Classes;
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
using QuanLyBenhVien.Classes;

namespace QuanLyBenhVien
{
    public partial class AccountForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public AccountForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT UserID AS[Tên đăng nhập], Pass AS[Mật khẩu] FROM USERLOGIN";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            adapter.Fill(dataSet, "USERLOGIN");
                            dgvAccount.DataSource = dataSet.Tables["USERLOGIN"];
                            dgvAccount.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu USERLOGIN: {ex.Message}");
                }
            }
        }

        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserID.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin UserID và Password.");
                return;
            }

            string query = @"
                IF EXISTS (SELECT 1 FROM USERLOGIN WHERE UserID = @UserID)
                    UPDATE USERLOGIN
                    SET Pass = @Pass
                    WHERE UserID = @UserID
                ELSE
                    INSERT INTO USERLOGIN (UserID, Pass, FLAG)
                    VALUES (@UserID, @Pass, @FLAG)";

            var parameters = new Dictionary<string, object>
            {
                {"@UserID", txtUserID.Text},
                {"@Pass", txtPassword.Text},
                {"@FLAG", 0}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserID.Text))
            {
                MessageBox.Show("Vui lòng nhập UserID cần xóa.");
                return;
            }

            string query = "DELETE FROM USERLOGIN WHERE UserID = @UserID";
            var parameters = new Dictionary<string, object>
            {
                {"@UserID", txtUserID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM USERLOGIN WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtUserID.Text))
                    {
                        query += " AND UserID = @UserID";
                        parameters.Add("@UserID", txtUserID.Text);
                    }

                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        query += " AND Pass = @Pass";
                        parameters.Add("@Pass", txtPassword.Text);
                    }

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvAccount.DataSource = dt;

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm UserLogin: {ex.Message}");
                }
            }
        }
        private void dgvAccount_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAccount.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvAccount.SelectedRows[0];
                txtUserID.Text = selectedRow.Cells[0].Value.ToString();
                txtPassword.Text = selectedRow.Cells[1].Value.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }
    }
}
