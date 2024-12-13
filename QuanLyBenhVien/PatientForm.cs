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
    public partial class PatientForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        public PatientForm()
        {
            InitializeComponent();
        }


        private void PatientForm_Load(object sender, EventArgs e)
        {
            LoadData();
            InitializeCmbRoomID();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT    bn.PatientID AS \"Mã bệnh nhân\", \r\n    bn.FullName AS \"Họ và tên\", \r\n    bn.DateOfBirth AS \"Ngày sinh\", \r\n    bn.Gender AS \"Giới tính\", \r\n    bn.PhoneNumber AS \"Số điện thoại\", \r\n    bn.AddressPatient AS \"Địa chỉ\", \r\n    bn.Email AS \"Email\", \r\n    bn.AdmissionDate AS \"Ngày nhập viện\", \r\n    bn.DischargeDate AS \"Ngày xuất viện\", \r\n    bn.RoomID AS \"Mã phòng\"\r\nFROM \r\n    PATIENT AS bn;\r\n";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset, "PATIENT");
                    dgvPatient.DataSource = dataset.Tables["PATIENT"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text) ||
                string.IsNullOrEmpty(dtpDateOfBirth.Text) || string.IsNullOrEmpty(cmbGender.Text) ||
                string.IsNullOrEmpty(txtPhoneNumber.Text) || string.IsNullOrEmpty(txtAddress.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(dtpAdmissionDate.Text) ||
                string.IsNullOrEmpty(dtpDischargeDate.Text))
            {
                return false;
            }


            if (CommonChecks.HasDigit(txtFullName.Text) ||
                !CommonChecks.IsEmail(txtEmail.Text) ||
                !CommonChecks.IsNumber(txtPhoneNumber.Text))
            {
                return false;
            }

            return true;
        }



        private void btnAddOrUpdatePatient_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM PATIENT WHERE PatientID = @PatientID)
                             UPDATE PATIENT SET FullName = @FullName, Gender = @Gender, DateOfBirth = @DateOfBirth,
                             PhoneNumber = @PhoneNumber, AddressPatient = @AddressPatient, Email = @Email,
                             AdmissionDate = @AdmissionDate, DischargeDate = @DischargeDate, RoomID = @RoomID
                             WHERE PatientID = @PatientID
                             ELSE
                             INSERT INTO PATIENT (PatientID, FullName, Gender, DateOfBirth, PhoneNumber, AddressPatient, Email, AdmissionDate, DischargeDate, RoomID)
                             VALUES (@PatientID, @FullName, @Gender, @DateOfBirth, @PhoneNumber, @AddressPatient, @Email, @AdmissionDate, @DischargeDate, @RoomID)";

            var parameters = new Dictionary<string, object>
            {
                {"@PatientID", txtPatientID.Text},
                {"@FullName", txtFullName.Text},
                {"@Gender", cmbGender.Text},
                {"@DateOfBirth", dtpDateOfBirth.Value},
                {"@PhoneNumber", txtPhoneNumber.Text},
                {"@AddressPatient", txtAddress.Text},
                {"@Email", txtEmail.Text},
                {"@AdmissionDate", dtpAdmissionDate.Value},
                {"@DischargeDate", dtpDischargeDate.Value},
                {"@RoomID", cmbRoomID.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            CommonControls.ResetInputFields(Parent);
            LoadData();
        }

        private void btnFindPatient_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT \r\n    bn.PatientID AS \"Mã bệnh nhân\", \r\n    bn.FullName AS \"Họ và tên\", \r\n    bn.DateOfBirth AS \"Ngày sinh\", \r\n    bn.Gender AS \"Giới tính\", \r\n    bn.PhoneNumber AS \"Số điện thoại\", \r\n    bn.AddressPatient AS \"Địa chỉ\", \r\n    bn.Email AS \"Email\", \r\n    bn.AdmissionDate AS \"Ngày nhập viện\", \r\n    bn.DischargeDate AS \"Ngày xuất viện\", \r\n    bn.RoomID AS \"Mã phòng\"\r\nFROM \r\n    PATIENT AS bn\r\n WHERE 1=1"; // 1=1 giúp nối dễ dàng các điều kiện
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtPatientID.Text))
                    {
                        query += " AND PatientID = @PatientID";
                        parameters.Add("@PatientID", txtPatientID.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(txtFullName.Text))
                    {
                        query += " AND FullName LIKE @FullName";
                        parameters.Add("@FullName", $"%{txtFullName.Text.Trim()}%");
                    }
                    if (!string.IsNullOrEmpty(cmbRoomID.Text))
                    {
                        query += " AND RoomID = @RoomID";
                        parameters.Add("@RoomID", cmbRoomID.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(cmbGender.Text))
                    {
                        query += " AND Gender = @Gender";
                        parameters.Add("@Gender", cmbGender.Text);
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

                        dgvPatient.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy bệnh nhân phù hợp với điều kiện tìm kiếm.",
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnRemovePatient_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM PATIENT WHERE PatientID = @PatientID";
            var parameters = new Dictionary<string, object> { { "@PatientID", txtPatientID.Text } };
            CommonQuery.ExecuteQuery(query, parameters);
            CommonControls.ResetInputFields(Parent);
            LoadData();
        }



        private void btnRefreshStaff_Click(object sender, EventArgs e)
        {
            CommonControls.ResetInputFields(Parent);
        }

        private void dgvPatient_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPatient.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvPatient.SelectedRows[0];
                txtPatientID.Text = selectedRow.Cells[0].Value.ToString();
                txtFullName.Text = selectedRow.Cells[1].Value.ToString();
                dtpDateOfBirth.Text = selectedRow.Cells[2].Value.ToString();
                cmbGender.Text = selectedRow.Cells[3].Value.ToString();
                txtPhoneNumber.Text = selectedRow.Cells[4].Value.ToString();
                txtAddress.Text = selectedRow.Cells[5].Value.ToString();
                txtEmail.Text = selectedRow.Cells[6].Value.ToString();
                dtpAdmissionDate.Text = selectedRow.Cells[7].Value.ToString();
                dtpDischargeDate.Text = selectedRow.Cells[8].Value.ToString();
                cmbRoomID.Text = selectedRow.Cells[9].Value.ToString();
            }
        }
    }


}

