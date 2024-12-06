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
using QuanLyBenhVien.Classes;

namespace QuanLyBenhVien
{
    public partial class EmployeeForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public EmployeeForm()
        {
            InitializeComponent();
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
                    string sql = "SELECT * FROM STAFF";
                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "STAFF");
                    dgvEmployee.DataSource = dataset.Tables["STAFF"];
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
                string.IsNullOrEmpty(txtSalary.Text) || string.IsNullOrEmpty(cmbDepartmentID.Text) ||
                string.IsNullOrEmpty(cmbGender.Text) || string.IsNullOrEmpty(cmbTypeOfStaff.Text))
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

        private void ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thực thi: {ex.Message}");
                }
            }
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

            ExecuteQuery(query, parameters);
            LoadData();
        }

        private void btnFindStaff_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Bắt đầu xây dựng truy vấn SQL động
                    string query = "SELECT * FROM STAFF WHERE 1=1"; // 1=1 giúp tránh lỗi cú pháp khi không có điều kiện
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

            ExecuteQuery(query, parameters);
            LoadData();
        }



        private void btnRefreshStaff_Click(object sender, EventArgs e)
        {
            txtStaffID.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
            txtSalary.Clear();
            cmbDepartmentID.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            cmbTypeOfStaff.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Today;
            dtpDateOfJoining.Value = DateTime.Today;
        }
    }
}
