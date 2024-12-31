using QuanLyBenhVien.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class WeeklyAssignmentForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public WeeklyAssignmentForm()
        {
            InitializeComponent();
            CommonControls.InitializeCmbStaffID(cmbStaffID);
            LoadData();
        }

        private void LoadData()
        {
            string query = "SELECT AssignmentID AS[Mã lịch trực], StaffID AS[Mã nhân viên], AssignmentDate AS [Ngày trực], ShiftType AS[Ca trực]  FROM WEEKLYASSIGNMENT";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet, "WEEKLYASSIGNMENT");
                    dgvWeeklyAssigment.DataSource = dataSet.Tables["WEEKLYASSIGNMENT"];
                    dgvWeeklyAssigment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);
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
                             AssignmentDate = @AssignmentDate
                             WHERE AssignmentID = @AssignmentID
                             ELSE
                             INSERT INTO WEEKLYASSIGNMENT (AssignmentID, StaffID, AssignmentDate, ShiftType)
                             VALUES (@AssignmentID, @StaffID, @AssignmentDate, @ShiftType)";

            var parameters = new Dictionary<string, object>
            {
                {"@AssignmentID", txtAssignmentID.Text},
                {"@StaffID", cmbStaffID.Text},
                {"@AssignmentDate", dtpAssignmentDate.Value},
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
            LoadData();
            CommonControls.ResetInputFields(Parent);
            txtAssignmentID.Clear();
            cmbStaffID.SelectedIndex = -1;
            cmbShiftType.Text = null;
            dtpAssignmentDate.Value = DateTime.Now;
        }

        private void dgvWeeklyAssignment_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvWeeklyAssigment.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvWeeklyAssigment.SelectedRows[0];
                txtAssignmentID.Text = selectedRow.Cells[0].Value.ToString();
                cmbStaffID.Text = selectedRow.Cells[1].Value.ToString();
                dtpAssignmentDate.Text = selectedRow.Cells[2].Value.ToString();
                cmbShiftType.Text = selectedRow.Cells[3].Value.ToString();
            }
        }

    }
}
