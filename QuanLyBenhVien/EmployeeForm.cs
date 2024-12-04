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
        public EmployeeForm()
        {
            InitializeComponent();
        }

        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        private SqlConnection conn;
        private SqlDataAdapter employeeAdapter;
        private DataSet employeeDataset;

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void OpenConnection()
        {
            if (conn == null)
                conn = new SqlConnection(connStr);

            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        private void CloseConnection()
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        private void LoadData()
        {
            try
            {
                OpenConnection();
                string sqlStr = "SELECT * FROM STAFF";

                employeeAdapter = new SqlDataAdapter(sqlStr, conn);
                employeeDataset = new DataSet();
                employeeAdapter.Fill(employeeDataset, "STAFF");

                dgvEmployee.DataSource = employeeDataset.Tables["STAFF"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

        private bool IsValid()
        {
            bool isValid = true;
            if (txtStaffID.Text.Length == 0 || txtFullName.Text.Length == 0 || txtEmail.Text.Length == 0
                || txtPhoneNumber.Text.Length == 0 || txtSalary.Text.Length == 0 || cmbDepartmentID.Text.Length == 0
                || cmbGender.Text.Length == 0 || cmbTypeOfStaff.Text.Length == 0)
                isValid = false;

            //Check FullName
            if(CommonChecks.HasDigit(txtFullName.Text))
                isValid = false;
            //Check Email
            if(!CommonChecks.IsEmail(txtEmail.Text))
                isValid = false;
            //Check PhoneNumber
            if (!CommonChecks.IsNumber(txtPhoneNumber.Text))
                isValid = false;
            //Check Salary
            if(!CommonChecks.IsNumber(txtSalary.Text))
                isValid = false;
            return isValid;
        }

        private void btnAddOrUpdateStaff_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Vui long nhap dung thong tin");
                return;
            }

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();

                string sqlStr = @"INSERT INTO STAFF values (@StaffID, @FullName, @TypeOfStaff, @Gender, @DateOfBirth, @PhoneNumber, @DateOfJoining, @Email, @Salary, @DepartmentID)";

                SqlCommand comm = new SqlCommand(sqlStr, conn);
                comm.Parameters.Add("@StaffID", SqlDbType.Char).Value = txtStaffID.Text;
                comm.Parameters.Add("@FullName", SqlDbType.NVarChar).Value = txtFullName.Text;
                comm.Parameters.Add("@TypeOfStaff", SqlDbType.NVarChar).Value = cmbTypeOfStaff.Text;
                comm.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = cmbGender.Text;
                comm.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = dtpBirthday.Value.Date;
                comm.Parameters.Add("@PhoneNumber", SqlDbType.VarChar).Value = txtPhoneNumber.Text;
                comm.Parameters.Add("@DateOfJoining", SqlDbType.Date).Value = dtpDateOfJoining.Value.Date;
                comm.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtEmail.Text;
                comm.Parameters.Add("@Salary", SqlDbType.Money).Value = txtSalary.Text;
                comm.Parameters.Add("@DepartmentID", SqlDbType.Char).Value = cmbDepartmentID.Text;

                int count = comm.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Staff added successfully");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to add staff");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
