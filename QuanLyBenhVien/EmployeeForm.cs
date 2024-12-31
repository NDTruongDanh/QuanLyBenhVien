using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLyBenhVien.Classes;

namespace QuanLyBenhVien
{
    public partial class EmployeeForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public EmployeeForm()
        {
            InitializeComponent();
            CommonControls.InitializeCmbDepartmentID(cmbDepartmentID);
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT \r\n    nv.StaffID AS \"Mã nhân viên\", \r\n    nv.FullName AS \"Họ và tên\", \r\n    nv.TypeOfStaff AS \"Loại nhân viên\", \r\n    nv.Gender AS \"Giới tính\", \r\n    nv.DateOfBirth AS \"Ngày sinh\", \r\n    nv.PhoneNumber AS \"Số điện thoại\", \r\n    nv.DateOfJoining AS \"Ngày vào làm\", \r\n    nv.Email AS \"Email\", \r\n    nv.Salary AS \"Lương\", \r\n    nv.DepartmentID AS \"Mã khoa\"\r\nFROM \r\n    STAFF AS nv;";
                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "STAFF");
                    dgvEmployee.DataSource = dataset.Tables["STAFF"];
                    dgvEmployee.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(txtStaffID.Text) || string.IsNullOrEmpty(txtFullName.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPhoneNumber.Text) ||
                string.IsNullOrEmpty(txtSalary.Text) || string.IsNullOrEmpty(cmbGender.Text) || 
                string.IsNullOrEmpty(cmbTypeOfStaff.Text))
            { 
                return false; 
            }


            if (CommonChecks.HasDigit(txtFullName.Text) ||
                !CommonChecks.IsEmail(txtEmail.Text) ||
                !CommonChecks.IsNumber(txtPhoneNumber.Text) ||
                !CommonChecks.IsNumber(txtSalary.Text))
            {
                return false;
            }

            return true;
        }



        private void btnAddOrUpdateStaff_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM STAFF WHERE StaffID = @StaffID)
                             UPDATE STAFF SET FullName = @FullName, TypeOfStaff = @TypeOfStaff, Gender = @Gender, 
                             DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber, DateOfJoining = @DateOfJoining, 
                             Email = @Email, Salary = @Salary, DepartmentID = @DepartmentID WHERE StaffID = @StaffID
                             ELSE
                             INSERT INTO STAFF (StaffID, FullName, TypeOfStaff, Gender, DateOfBirth, PhoneNumber, DateOfJoining, Email, Salary, DepartmentID)
                             VALUES (@StaffID, @FullName, @TypeOfStaff, @Gender, @DateOfBirth, @PhoneNumber, @DateOfJoining, @Email, @Salary, @DepartmentID)";

            var parameters = new Dictionary<string, object>
            {
                {"@StaffID", txtStaffID.Text},
                {"@FullName", txtFullName.Text},
                {"@TypeOfStaff", cmbTypeOfStaff.Text},
                {"@Gender", cmbGender.Text},
                {"@DateOfBirth", dtpBirthday.Value.Date},
                {"@PhoneNumber", txtPhoneNumber.Text},
                {"@DateOfJoining", dtpDateOfJoining.Value.Date},
                {"@Email", txtEmail.Text},
                {"@Salary", txtSalary.Text},
                {"@DepartmentID", cmbDepartmentID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFindStaff_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Bắt đầu xây dựng truy vấn SQL động
                    string query = "SELECT \r\n    nv.StaffID AS \"Mã nhân viên\", \r\n    nv.FullName AS \"Họ và tên\", \r\n    nv.TypeOfStaff AS \"Loại nhân viên\", \r\n    nv.Gender AS \"Giới tính\", \r\n    nv.DateOfBirth AS \"Ngày sinh\", \r\n    nv.PhoneNumber AS \"Số điện thoại\", \r\n    nv.DateOfJoining AS \"Ngày vào làm\", \r\n    nv.Email AS \"Email\", \r\n    nv.Salary AS \"Lương\", \r\n    nv.DepartmentID AS \"Mã khoa\"\r\nFROM \r\n    STAFF AS nv\r\n WHERE 1=1"; // 1=1 giúp tránh lỗi cú pháp khi không có điều kiện
                    var parameters = new Dictionary<string, object>();

                    // Kiểm tra từng trường và thêm vào điều kiện truy vấn nếu không rỗng
                    if (!string.IsNullOrEmpty(txtStaffID.Text))
                    {
                        query += " AND StaffID = @StaffID";
                        parameters.Add("@StaffID", txtStaffID.Text);
                    }
                    if (!string.IsNullOrEmpty(txtFullName.Text))
                    {
                        query += " AND FullName LIKE @FullName";
                        parameters.Add("@FullName", $"%{txtFullName.Text}%"); // Sử dụng LIKE để tìm kiếm gần đúng
                    }
                    if (!string.IsNullOrEmpty(txtSalary.Text))
                    {
                        query += " AND Salary = @Salary";
                        parameters.Add("@Salary", txtSalary.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbGender.Text))
                    {
                        query += " AND Gender = @Gender";
                        parameters.Add("@Gender", cmbGender.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbTypeOfStaff.Text))
                    {
                        query += " AND TypeOfStaff = @TypeOfStaff";
                        parameters.Add("@TypeOfStaff", cmbTypeOfStaff.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbDepartmentID.Text))
                    {
                        query += " AND DepartmentID = @DepartmentID";
                        parameters.Add("@DepartmentID", cmbDepartmentID.Text);
                    }

                    // Thực hiện truy vấn
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);

                        // Hiển thị kết quả
                        dgvEmployee.DataSource = resultTable;

                        // Thông báo nếu không có kết quả
                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp với điều kiện tìm kiếm.",
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}",
                                    "Lỗi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }


        private void btnRemoveStaff_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM STAFF WHERE StaffID = @StaffID";
            var parameters = new Dictionary<string, object> { { "@StaffID", txtStaffID.Text } };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
        }
    
        private void btnRefreshStaff_Click(object sender, EventArgs e)
        {
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void dgvEmployee_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmployee.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvEmployee.SelectedRows[0];

                txtStaffID.Text = selectedRow.Cells[0].Value.ToString();
                txtFullName.Text = selectedRow.Cells[1].Value.ToString();
                cmbTypeOfStaff.Text = selectedRow.Cells[2].Value.ToString();
                cmbGender.Text = selectedRow.Cells[3].Value.ToString();
                dtpBirthday.Text = selectedRow.Cells[4].Value.ToString();
                txtPhoneNumber.Text = selectedRow.Cells[5].Value.ToString();
                dtpDateOfJoining.Text = selectedRow.Cells[6].Value.ToString();
                txtEmail.Text = selectedRow.Cells[7].Value.ToString();
                txtSalary.Text = selectedRow.Cells[8].Value.ToString();
                cmbDepartmentID.Text = selectedRow.Cells[9].Value.ToString();
            }
        }

        private void dgvEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtStaffID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbDepartmentID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpDateOfJoining_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtpBirthday_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtFullName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
