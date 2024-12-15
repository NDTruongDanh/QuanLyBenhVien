using QuanLyBenhVien.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class WeeklyAssignmentForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public WeeklyAssignmentForm()
        {
            InitializeComponent();
            LoadDepartmentIDs();
            LoadStaffID();
            LoadData();
        }

        private void LoadDepartmentIDs()
        {
            string query = "SELECT DepartmentID FROM DEPARTMENT";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbDepartmentID.Items.Add(reader["DepartmentID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadStaffID()
        {
            string query = "SELECT StaffID FROM STAFF";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbStaffID.Items.Add(reader["StaffID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void LoadData()
        {
            string query = "SELECT * FROM WEEKLYASSIGNMENT";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "WEEKLYASSIGNMENT");
                    dgvWeeklyAssigment.DataSource = dataSet.Tables["WEEKLYASSIGNMENT"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(txtAssignmentID.Text) &&
                   !string.IsNullOrEmpty(cmbStaffID.Text) &&
                   !string.IsNullOrEmpty(cmbDepartmentID.Text) &&
                   !string.IsNullOrEmpty(cmbShiftType.Text);
        }

        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM WEEKLYASSIGNMENT WHERE AssignmentID = @AssignmentID)
                             UPDATE WEEKLYASSIGNMENT SET StaffID = @StaffID, ShiftType = @ShiftType, 
                             WeekStartDate = @WeekStartDate, WeekEndDate = @WeekEndDate, DepartmentID = @DepartmentID
                             WHERE AssignmentID = @AssignmentID
                             ELSE
                             INSERT INTO WEEKLYASSIGNMENT (AssignmentID, StaffID, DepartmentID, WeekStartDate, WeekEndDate, ShiftType)
                             VALUES (@AssignmentID, @StaffID, @DepartmentID, @WeekStartDate, @WeekEndDate, @ShiftType)";

            var parameters = new Dictionary<string, object>
            {
                {"@AssignmentID", txtAssignmentID.Text},
                {"@StaffID", cmbStaffID.Text},
                {"@DepartmentID", cmbDepartmentID.Text},
                {"@WeekStartDate", dtpWeekStartDate.Value},
                {"@WeekEndDate", dtpWeekEndDate.Value},
                {"@ShiftType", cmbShiftType.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM WEEKLYASSIGNMENT WHERE 1=1";
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(txtAssignmentID.Text))
            {
                query += " AND AssignmentID = @AssignmentID";
                parameters.Add("@AssignmentID", txtAssignmentID.Text);
            }
            if (!string.IsNullOrEmpty(cmbDepartmentID.Text))
            {
                query += " AND DepartmentID = @DepartmentID";
                parameters.Add("@DepartmentID", cmbDepartmentID.Text);
            }
            if (!string.IsNullOrEmpty(cmbStaffID.Text))
            {
                query += " AND StaffID = @StaffID";
                parameters.Add("@StaffID", cmbStaffID.Text);
            }
            if (!string.IsNullOrEmpty(cmbShiftType.Text))
            {
                query += " AND ShiftType = @ShiftType";
                parameters.Add("@ShiftType", cmbShiftType.Text);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable resultTable = new DataTable();
                    dataAdapter.Fill(resultTable);
                    dgvWeeklyAssigment.DataSource = resultTable;

                    if (resultTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}");
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM WEEKLYASSIGNMENT WHERE AssignmentID = @AssignmentID";
            var parameters = new Dictionary<string, object> { { "@AssignmentID", txtAssignmentID.Text } };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
           CommonControls.ResetInputFields(Parent);
            txtAssignmentID.Clear();
            cmbStaffID.SelectedIndex = -1;
            cmbDepartmentID.SelectedIndex = -1;
            cmbShiftType.Text = null;
            dtpWeekEndDate.Value = DateTime.Now;
            dtpWeekStartDate.Value = DateTime.Now;
        }

        private void dgvWeeklyAssignment_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWeeklyAssigment.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvWeeklyAssigment.SelectedRows[0];
                txtAssignmentID.Text = selectedRow.Cells[0].Value.ToString();
                cmbStaffID.Text = selectedRow.Cells[1].Value.ToString();
                cmbDepartmentID.Text = selectedRow.Cells[2].Value.ToString();
                dtpWeekStartDate.Text = selectedRow.Cells[3].Value.ToString();
                dtpWeekEndDate.Text = selectedRow.Cells[4].Value.ToString();
                cmbShiftType.Text = selectedRow.Cells[5].Value.ToString();
            }
        }
    }
}
