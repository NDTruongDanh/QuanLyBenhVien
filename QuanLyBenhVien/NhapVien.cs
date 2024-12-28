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
    public partial class NhapVien : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public NhapVien()
        {
            InitializeComponent();
            LoadHospitalizations();
            CommonControls.InitializeCmbPatientID(cmbPatientID);
            CommonControls.InitializeCmbRoomID(cmbRoomID);
        }

        private void LoadHospitalizations()
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"
                        SELECT 
                            HospitalizationID AS [Mã nhập viện],
                            PatientID AS [Mã bệnh nhân],
                            RoomID AS [Mã phòng],
                            AdmissionDate AS [Ngày nhập viện],
                            DischargeDate AS [Ngày xuất viện]
                        FROM HOSPITALIZATION";

                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "HOSPITALIZATION");
                    dgvHospitalization.DataSource = dataset.Tables["HOSPITALIZATION"];
                    dgvHospitalization.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu nhập viện: {ex.Message}");
                }
            }
        }

        private bool IsValidHospitalization()
        {
            if (string.IsNullOrEmpty(txtHospitalizationID.Text) ||
                string.IsNullOrEmpty(cmbPatientID.Text) ||
                string.IsNullOrEmpty(cmbRoomID.Text))
            {
                return false;
            }

            return true;
        }

        private void btnAddOrUpdateHospitalization_Click(object sender, EventArgs e)
        {
            if (!IsValidHospitalization())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM HOSPITALIZATION WHERE HospitalizationID = @HospitalizationID)
                             UPDATE HOSPITALIZATION 
                             SET PatientID = @PatientID, RoomID = @RoomID, 
                                 AdmissionDate = @AdmissionDate, DischargeDate = @DischargeDate
                             WHERE HospitalizationID = @HospitalizationID
                             ELSE
                             INSERT INTO HOSPITALIZATION 
                             (HospitalizationID, PatientID, RoomID, AdmissionDate, DischargeDate)
                             VALUES 
                             (@HospitalizationID, @PatientID, @RoomID, @AdmissionDate, @DischargeDate)";

            var parameters = new Dictionary<string, object>
            {
                {"@HospitalizationID", txtHospitalizationID.Text},
                {"@PatientID", cmbPatientID.Text},
                {"@RoomID", cmbRoomID.Text},
                {"@AdmissionDate", dtpAdmissionDate.Value},
                {"@DischargeDate", dtpDischargeDate.Value}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadHospitalizations();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRemoveHospitalization_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHospitalizationID.Text))
            {
                MessageBox.Show("Vui lòng chọn một mục nhập viện để xóa.");
                return;
            }

            string query = "DELETE FROM HOSPITALIZATION WHERE HospitalizationID = @HospitalizationID";
            var parameters = new Dictionary<string, object>
            {
                {"@HospitalizationID", txtHospitalizationID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadHospitalizations();
        }

        private void btnFindHospitalization_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM HOSPITALIZATION WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtHospitalizationID.Text))
                    {
                        query += " AND HospitalizationID = @HospitalizationID";
                        parameters.Add("@HospitalizationID", txtHospitalizationID.Text);
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

                        dgvHospitalization.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm nhập viện: {ex.Message}");
                }
            }
        }

        private void dgvHospitalization_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHospitalization.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvHospitalization.SelectedRows[0];
                txtHospitalizationID.Text = selectedRow.Cells[0].Value.ToString();
                cmbPatientID.Text = selectedRow.Cells[1].Value.ToString();
                cmbRoomID.Text = selectedRow.Cells[2].Value.ToString();
                dtpAdmissionDate.Value = DateTime.Parse(selectedRow.Cells[3].Value.ToString());
                dtpDischargeDate.Value = string.IsNullOrEmpty(selectedRow.Cells[4].Value.ToString())
                    ? DateTime.Now
                    : DateTime.Parse(selectedRow.Cells[4].Value.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CommonControls.ResetInputFields(Parent);
        }
    }
}
