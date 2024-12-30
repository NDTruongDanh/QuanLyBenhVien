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
    public partial class Appointment : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public Appointment()
        {
            InitializeComponent();
            CommonControls.InitializeCmbDoctorID(cmbDoctorID);
            CommonControls.InitializeCmbPatientID(cmbPatientID);
            CommonControls.InitializeCmbDepartmentID(cmbDepartmentID);
            LoadAppointments();
            SetupDateTimePickerCustom();
        }

        private void SetupDateTimePickerCustom()
        {
            dtpAppointmentDateTime.Format = DateTimePickerFormat.Custom;
            dtpAppointmentDateTime.CustomFormat = "hh:mm tt dd/MM/yyyy"; // Định dạng ngày và giờ
            dtpAppointmentDateTime.ShowUpDown = true; // Ẩn lịch, chỉ chọn giờ
        }

        private void LoadAppointments()
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"
                SELECT 
                    AppointmentID AS [Mã cuộc hẹn],
                    PatientID AS [Mã bệnh nhân],
                    DoctorID AS [Mã bác sĩ],
                    DepartmentID AS [Mã khoa],
                    AppointmentDateTime AS [Thời gian hẹn khám],
                    AppointmentStatus AS [Trạng thái cuộc hẹn]
                FROM APPOINTMENT";

                    var adapter = new SqlDataAdapter(sql, conn);
                    var dataset = new DataSet();
                    adapter.Fill(dataset, "APPOINTMENT");
                    dgvAppointment.DataSource = dataset.Tables["APPOINTMENT"];
                    dgvAppointment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu cuộc hẹn: {ex.Message}");
                }
            }
        }

        private bool IsValidAppointment()
        {
            if (string.IsNullOrEmpty(txtAppointmentID.Text) ||
                string.IsNullOrEmpty(cmbPatientID.Text) ||
                string.IsNullOrEmpty(cmbDoctorID.Text) ||
                string.IsNullOrEmpty(cmbDepartmentID.Text))
            {
                return false;
            }

            return true;
        }

        private void btnAddOrUpdateAppointment_Click(object sender, EventArgs e)
        {
            if (!IsValidAppointment())
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM APPOINTMENT WHERE AppointmentID = @AppointmentID)
                     UPDATE APPOINTMENT 
                     SET PatientID = @PatientID, DoctorID = @DoctorID, DepartmentID = @DepartmentID, 
                         AppointmentDateTime = @AppointmentDateTime
                     WHERE AppointmentID = @AppointmentID
                     ELSE
                     INSERT INTO APPOINTMENT 
                     (AppointmentID, PatientID, DoctorID, DepartmentID, AppointmentDateTime, AppointmentStatus)
                     VALUES 
                     (@AppointmentID, @PatientID, @DoctorID, @DepartmentID, @AppointmentDateTime, N'Đang chờ xử lý')";

                    var parameters = new Dictionary<string, object>
            {
                {"@AppointmentID", txtAppointmentID.Text},
                {"@PatientID", cmbPatientID.Text},
                {"@DoctorID", cmbDoctorID.Text},
                {"@DepartmentID", cmbDepartmentID.Text},
                {"@AppointmentDateTime", dtpAppointmentDateTime.Value}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadAppointments();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnRemoveAppointment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAppointmentID.Text))
            {
                MessageBox.Show("Vui lòng chọn một cuộc hẹn để xóa.");
                return;
            }

            string query = "DELETE FROM APPOINTMENT WHERE AppointmentID = @AppointmentID";
            var parameters = new Dictionary<string, object>
            {
                {"@AppointmentID", txtAppointmentID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadAppointments();
        }

        private void btnFindAppointment_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM APPOINTMENT WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtAppointmentID.Text))
                    {
                        query += " AND AppointmentID = @AppointmentID";
                        parameters.Add("@AppointmentID", txtAppointmentID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbPatientID.Text))
                    {
                        query += " AND PatientID = @PatientID";
                        parameters.Add("@PatientID", cmbPatientID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbDoctorID.Text))
                    {
                        query += " AND DoctorID = @DoctorID";
                        parameters.Add("@DoctorID", cmbDoctorID.Text);
                    }
                    if (!string.IsNullOrEmpty(cmbDepartmentID.Text))
                    {
                        query += " AND DepartmentID = @DepartmentID";
                        parameters.Add("@DepartmentID", cmbDepartmentID.Text);
                    }


                    string status = Status();
                    query += $" AND AppointmentStatus = @AppointmentStatus";
                    parameters.Add("@AppointmentStatus", status);
                   

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);

                        dgvAppointment.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm cuộc hẹn: {ex.Message}");
                }
            }
        }

        private void dgvAppointment_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAppointment.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvAppointment.SelectedRows[0];
                txtAppointmentID.Text = selectedRow.Cells[0].Value.ToString();
                cmbPatientID.Text = selectedRow.Cells[1].Value.ToString();
                cmbDoctorID.Text = selectedRow.Cells[2].Value.ToString();
                cmbDepartmentID.Text = selectedRow.Cells[3].Value.ToString();
                dtpAppointmentDateTime.Text = selectedRow.Cells[4].Value.ToString();

                if (selectedRow.Cells[5].Value.ToString() == "Chấp thuận")
                    rbtnAccept.Checked = true;
                else if(selectedRow.Cells[5].Value.ToString() == "Từ chối")
                    rbtnDecline.Checked = true;
                else
                {
                    rbtnDecline.Checked = false;
                    rbtnAccept.Checked = false;
                }
            }
        }

        private void rbtnAccept_Click(object sender, EventArgs e)
        {
            rbtnAccept.Checked = !rbtnAccept.Checked;
        }

        private void rbtnDecline_Click(object sender, EventArgs e)
        {
            rbtnDecline.Checked = !rbtnDecline.Checked;
        }

        private void rbtnDecline_CheckedChanged(object sender, EventArgs e)
        {
            if(rbtnDecline.Checked)
                rbtnAccept.Checked = false;
        }

        private void rbtnAccept_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAccept.Checked)
                rbtnDecline.Checked = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
            rbtnAccept.Checked = false;
            rbtnDecline.Checked = false;
            CommonControls.ResetInputFields(Parent);
        }

        private string Status()
        {
            if (rbtnAccept.Checked)
                return "Chấp thuận";
            else if (rbtnDecline.Checked)
                return "Từ chối";
            return "Đang chờ xử lý";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string query = @"UPDATE APPOINTMENT 
                         SET AppointmentStatus = @AppointmentStatus
                         WHERE AppointmentID = @AppointmentID";
                       
            

            var parameters = new Dictionary<string, object>
            {
                {"@AppointmentID", txtAppointmentID.Text},
                {"@AppointmentStatus", Status()}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadAppointments();
            CommonControls.ResetInputFields(Parent);
        }

        private void cmbDoctorID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbDepartmentID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtAppointmentID_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpAppointmentDateTime_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
