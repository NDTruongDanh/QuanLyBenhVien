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
    public partial class DepartmentForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        public DepartmentForm()
        {
            InitializeComponent();
        }

        private void DepartmentForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM DEPARTMENT";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "DEPARTMENT");
                    dgvDepartment.DataSource = dataSet.Tables["DEPARTMENT"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
            }
        }

        private bool IsValid()
        {
            if(string.IsNullOrEmpty(txtDepartmentID.Text) || string.IsNullOrEmpty(txtDepartmentName.Text)
                || string.IsNullOrEmpty(txtPhoneNumber.Text) || string.IsNullOrEmpty(txtEmployeeNumber.Text)
                || string.IsNullOrEmpty(txtLocation.Text))
                return false;

            if(CommonChecks.HasDigit(txtDepartmentName.Text) 
                || !CommonChecks.IsNumber(txtPhoneNumber.Text)
                || !CommonChecks.IsNumber(txtEmployeeNumber.Text))
                return false;
            return true;
        }

        private void btnAddOrUpdateDepartment_Click(object sender, EventArgs e)
        {
            if(!IsValid())
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin!", "Nhập thông tin bị lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM DEPARTMENT WHERE DepartmentID = @DepartmentID)
                     UPDATE DEPARTMENT SET DepartmentName = @DepartmentName, EmployeeNumber = @EmployeeNumber, 
                     HeadDepartmentID = @HeadDepartmentID, PhoneNumber = @PhoneNumber, LocationDPM = @LocationDPM 
                     WHERE DepartmentID = @DepartmentID
                     ELSE
                     INSERT INTO DEPARTMENT (DepartmentID, DepartmentName, EmployeeNumber, HeadDepartmentID, PhoneNumber, LocationDPM)
                     VALUES (@DepartmentID, @DepartmentName, @EmployeeNumber, @HeadDepartmentID, @PhoneNumber, @LocationDPM)";
            var parameters = new Dictionary<string, object>
            {
                { "@DepartmentID", txtDepartmentID.Text },
                { "@DepartmentName", txtDepartmentName.Text },
                { "@EmployeeNumber", txtEmployeeNumber.Text },
                { "@HeadDepartmentID", txtHeadDepartmentID.Text },
                { "@PhoneNumber", txtPhoneNumber.Text },
                { "@LocationDPM", txtLocation.Text }
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFindDepartment_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM DEPARTMENT WHERE 1=1"; // 1=1 tránh lỗi cú pháp khi không có điều kiện
                    Dictionary<string, object> parameters = new Dictionary<string, object>();


                    if (!string.IsNullOrEmpty(txtDepartmentID.Text))
                    {
                        query += " AND DepartmentID = @DepartmentID";
                        parameters.Add("@DepartmentID", txtDepartmentID.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(txtDepartmentName.Text))
                    {
                        query += " AND DepartmentName LIKE @DepartmentName";
                        parameters.Add("@DepartmentName", $"%{txtDepartmentName.Text.Trim()}%"); // Sử dụng LIKE để tìm kiếm gần đúng
                    }
                    if (!string.IsNullOrEmpty(txtHeadDepartmentID.Text))
                    {
                        query += " AND HeadDepartmentID = @HeadDepartmentID";
                        parameters.Add("@HeadDepartmentID", txtHeadDepartmentID.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(txtPhoneNumber.Text))
                    {
                        query += " AND PhoneNumber = @PhoneNumber";
                        parameters.Add("@PhoneNumber", txtPhoneNumber.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(txtLocation.Text))
                    {
                        query += " AND LocationDPM LIKE @LocationDPM";
                        parameters.Add("@LocationDPM", $"%{txtLocation.Text.Trim()}%");
                    }

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);

                        dgvDepartment.DataSource = resultTable;


                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy phòng ban nào phù hợp với điều kiện tìm kiếm.",
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}",
                                    "Lỗi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void btnRemoveDepartment_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM DEPARTMENT WHERE DepartmentID = @DepartmentID";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@DepartmentID", txtDepartmentID.Text } };
            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
        }

        private void btnRefreshDepartment_Click(object sender, EventArgs e)
        {
            CommonControls.ResetInputFields(Parent);
        }

        private void dgvDepartment_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDepartment.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvDepartment.SelectedRows[0];

                txtDepartmentID.Text = selectedRow.Cells[0].Value.ToString();
                txtDepartmentName.Text = selectedRow.Cells[1].Value.ToString();
                txtEmployeeNumber.Text = selectedRow.Cells[2].Value.ToString();
                txtHeadDepartmentID.Text = selectedRow.Cells[3].Value.ToString();
                txtPhoneNumber.Text = selectedRow.Cells[4].Value.ToString();
                txtLocation.Text = selectedRow.Cells[5].Value.ToString();
            }
        }
    }
}
