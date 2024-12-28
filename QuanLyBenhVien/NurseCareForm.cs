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

namespace QuanLyBenhVien
{
    public partial class NurseCareForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public NurseCareForm()
        {
            InitializeComponent();
            InitializeCmbPatientID();
            InitializeCmbNurseID();
            InitializeCmbRoomID();
            LoadNurseCareData();
        }

        private void InitializeCmbPatientID()
        {
            string query = "SELECT PatientID FROM PATIENT";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbPatientID.Items.Add(reader["PatientID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void InitializeCmbNurseID()
        {
            string query = "SELECT StaffID FROM STAFF WHERE TypeOfStaff LIKE N'%Điều dưỡng%'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbNurseID.Items.Add(reader["StaffID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void InitializeCmbRoomID()
        {
            string query = "SELECT RoomID FROM ROOM";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbRoomID.Items.Add(reader["RoomID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadNurseCareData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"
                    SELECT 
                        CareID AS [Mã chăm sóc],
                        NurseID AS [Mã điều dưỡng],
                        PatientID AS [Mã bệnh nhân],
                        RoomID AS [Mã phòng],
                        CareDateTime AS [Ngày bắt đầu chăm sóc],
                        CareType AS [Loại chăm sóc],
                        Notes AS [Ghi chú]
                    FROM NURSECARE";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        using (DataSet dataset = new DataSet())
                        {
                            adapter.Fill(dataset, "NURSECARE");
                            dgvNurseCare.DataSource = dataset.Tables["NURSECARE"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu chăm sóc: {ex.Message}");
                }
            }
        }

        private bool IsValidNurseCare()
        {
            if (string.IsNullOrEmpty(txtCareID.Text) ||
                string.IsNullOrEmpty(cmbNurseID.Text) ||
                string.IsNullOrEmpty(cmbPatientID.Text) ||
                string.IsNullOrEmpty(cmbRoomID.Text) ||
                string.IsNullOrEmpty(cmbCaretype.Text))
            {
                return false;
            }

            return true;
        }

        private void btnAddOrUpdateNurseCare_Click(object sender, EventArgs e)
        {
            if (!IsValidNurseCare())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM NURSECARE WHERE CareID = @CareID)
                             UPDATE NURSECARE 
                             SET NurseID = @NurseID, PatientID = @PatientID, RoomID = @RoomID, 
                                 CareDateTime = @CareDateTime, CareType = @CareType, Notes = @Notes
                             WHERE CareID = @CareID
                             ELSE
                             INSERT INTO NURSECARE 
                             (CareID, NurseID, PatientID, RoomID, CareDateTime, CareType, Notes)
                             VALUES 
                             (@CareID, @NurseID, @PatientID, @RoomID, @CareDateTime, @CareType, @Notes)";

            var parameters = new Dictionary<string, object>
            {
                {"@CareID", txtCareID.Text},
                {"@NurseID", cmbNurseID.Text},
                {"@PatientID", cmbPatientID.Text},
                {"@RoomID", cmbRoomID.Text},
                {"@CareDateTime", dtpCareDateTime.Value},
                {"@CareType", cmbCaretype.Text},
                {"@Notes", txtNotes.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadNurseCareData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRemoveNurseCare_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCareID.Text))
            {
                MessageBox.Show("Vui lòng chọn một bản ghi chăm sóc để xóa.");
                return;
            }

            string query = "DELETE FROM NURSECARE WHERE CareID = @CareID";
            var parameters = new Dictionary<string, object>
            {
                {"@CareID", txtCareID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadNurseCareData();
        }

        private void btnFindNurseCare_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM NURSECARE WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtCareID.Text))
                    {
                        query += " AND CareID = @CareID";
                        parameters.Add("@CareID", txtCareID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbNurseID.Text))
                    {
                        query += " AND NurseID = @NurseID";
                        parameters.Add("@NurseID", cmbNurseID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbPatientID.Text))
                    {
                        query += " AND PatientID = @PatientID";
                        parameters.Add("@PatientID", cmbPatientID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbRoomID.Text))
                    {
                        query += " AND RoomID = @RoomID";
                        parameters.Add("@RoomID", cmbRoomID.Text);
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

                        dgvNurseCare.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm chăm sóc: {ex.Message}");
                }
            }
        }

        private void dgvNurseCare_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNurseCare.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvNurseCare.SelectedRows[0];
                txtCareID.Text = selectedRow.Cells[0].Value.ToString();
                cmbNurseID.Text = selectedRow.Cells[1].Value.ToString();
                cmbPatientID.Text = selectedRow.Cells[2].Value.ToString();
                cmbRoomID.Text = selectedRow.Cells[3].Value.ToString();
                dtpCareDateTime.Text = selectedRow.Cells[4].Value.ToString();
                cmbCaretype.Text = selectedRow.Cells[5].Value.ToString();
                txtNotes.Text = selectedRow.Cells[6].Value.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadNurseCareData();
            CommonControls.ResetInputFields(Parent);
        }
    }
}
    
