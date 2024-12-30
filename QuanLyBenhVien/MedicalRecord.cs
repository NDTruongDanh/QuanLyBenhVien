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
    public partial class MedicalRecord : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public MedicalRecord()
        {
            InitializeComponent();
            LoadMedicalRecords();
            CommonControls.InitializeCmbPatientID(cmbPatientID);
            CommonControls.InitializeCmbDoctorID(cmbDoctorID);    
        }

        private void LoadMedicalRecords()
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT \r\n    hs.RecordID AS \"Mã hồ sơ\", \r\n    hs.PatientID AS \"Mã bệnh nhân\", \r\n    hs.DoctorID AS \"Mã bác sĩ\", \r\n    hs.VisitDate AS \"Ngày khám\", \r\n    hs.Diagnosis AS \"Chẩn đoán\", \r\n    hs.TestResults AS \"Kết quả xét nghiệm\", \r\n    hs.TreatmentPlan AS \"Kế hoạch điều trị\"\r\nFROM \r\n    MEDICALRECORD AS hs;\r\n";
                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "MEDICALRECORD");
                    dgvMedicalRecord.DataSource = dataset.Tables["MEDICALRECORD"];
                    dgvMedicalRecord.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
            }
        }

        private bool IsValidMedicalRecord()
        {
            if (string.IsNullOrEmpty(txtRecordID.Text) ||
                string.IsNullOrEmpty(cmbPatientID.Text) ||
                string.IsNullOrEmpty(cmbDoctorID.Text) ||
                string.IsNullOrEmpty(txtDiagnosis.Text) ||
                string.IsNullOrEmpty(txtTestResults.Text) ||
                string.IsNullOrEmpty(txtTreatmentPlan.Text))
            {
                return false;
            }

            return true;
        }

        private void btnAddOrUpdateMedicalRecord_Click(object sender, EventArgs e)
        {
            if (!IsValidMedicalRecord())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM MEDICALRECORD WHERE RecordID = @RecordID)
                     UPDATE MEDICALRECORD SET PatientID = @PatientID, DoctorID = @DoctorID, VisitDate = @VisitDate, 
                     Diagnosis = @Diagnosis, TestResults = @TestResults, TreatmentPlan = @TreatmentPlan
                     WHERE RecordID = @RecordID
                     ELSE
                     INSERT INTO MEDICALRECORD (RecordID, PatientID, DoctorID, VisitDate, Diagnosis, TestResults, TreatmentPlan)
                     VALUES (@RecordID, @PatientID, @DoctorID, @VisitDate, @Diagnosis, @TestResults, @TreatmentPlan)";

            var parameters = new Dictionary<string, object>
            {
                {"@RecordID", txtRecordID.Text},
                {"@PatientID", cmbPatientID.Text},
                {"@DoctorID", cmbDoctorID.Text},
                {"@VisitDate", dtpVisitDate.Value.Date},
                {"@Diagnosis", txtDiagnosis.Text},
                {"@TestResults", txtTestResults.Text},
                {"@TreatmentPlan", txtTreatmentPlan.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadMedicalRecords();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRemoveMedicalRecord_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRecordID.Text))
            {
                MessageBox.Show("Vui lòng chọn một hồ sơ để xóa.");
                return;
            }

            string query = "DELETE FROM MEDICALRECORD WHERE RecordID = @RecordID";
            var parameters = new Dictionary<string, object>
            {
                {"@RecordID", txtRecordID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadMedicalRecords();
        }

        private void btnFindMedicalRecord_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT \r\n    hs.RecordID AS \"Mã hồ sơ\", \r\n    hs.PatientID AS \"Mã bệnh nhân\", \r\n    hs.DoctorID AS \"Mã bác sĩ\", \r\n    hs.VisitDate AS \"Ngày khám\", \r\n    hs.Diagnosis AS \"Chẩn đoán\", \r\n    hs.TestResults AS \"Kết quả xét nghiệm\", \r\n    hs.TreatmentPlan AS \"Kế hoạch điều trị\"\r\nFROM \r\n    MEDICALRECORD AS hs\r\n WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtRecordID.Text))
                    {
                        query += " AND RecordID = @RecordID";
                        parameters.Add("@RecordID", txtRecordID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbPatientID.Text))
                    {
                        query += " AND PatientID = @PatientID";
                        parameters.Add("@PatientID", cmbPatientID.Text);
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

                        dgvMedicalRecord.DataSource = resultTable;

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
        }




        private void dgvMedicalRecord_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMedicalRecord.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvMedicalRecord.SelectedRows[0];
                txtRecordID.Text = selectedRow.Cells[0].Value.ToString();
                cmbPatientID.Text = selectedRow.Cells[1].Value.ToString();
                cmbDoctorID.Text = selectedRow.Cells[2].Value.ToString();
                dtpVisitDate.Text = selectedRow.Cells[3].Value.ToString();
                txtDiagnosis.Text = selectedRow.Cells[4].Value.ToString();
                txtTestResults.Text = selectedRow.Cells[5].Value.ToString();
                txtTreatmentPlan.Text = selectedRow.Cells[6].Value.ToString();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMedicalRecords();
            CommonControls.ResetInputFields(Parent);
        }
    }
}
