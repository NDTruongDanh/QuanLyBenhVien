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
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT   bn.PatientID AS ""Mã bệnh nhân"", 
                                            bn.FullName AS ""Họ và tên"", 
                                            bn.DateOfBirth AS ""Ngày sinh"", 
                                            bn.Gender AS ""Giới tính"", 
                                            bn.PhoneNumber AS ""Số điện thoại"", 
                                            bn.AddressPatient AS ""Địa chỉ"", 
                                            bn.Email AS ""Email""
                                        FROM 
                                            PATIENT AS bn;";
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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(txtPatientID.Text) || string.IsNullOrEmpty(txtFullName.Text) ||
                string.IsNullOrEmpty(dtpDateOfBirth.Text) || string.IsNullOrEmpty(cmbGender.Text) ||
                string.IsNullOrEmpty(txtPhoneNumber.Text) || string.IsNullOrEmpty(txtAddress.Text) ||
                string.IsNullOrEmpty(txtEmail.Text))
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
                             PhoneNumber = @PhoneNumber, AddressPatient = @AddressPatient, Email = @Email
                             WHERE PatientID = @PatientID
                             ELSE
                             INSERT INTO PATIENT (PatientID, FullName, Gender, DateOfBirth, PhoneNumber, AddressPatient, Email)
                             VALUES (@PatientID, @FullName, @Gender, @DateOfBirth, @PhoneNumber, @AddressPatient, @Email)";

            var parameters = new Dictionary<string, object>
            {
                {"@PatientID", txtPatientID.Text},
                {"@FullName", txtFullName.Text},
                {"@Gender", cmbGender.Text},
                {"@DateOfBirth", dtpDateOfBirth.Value},
                {"@PhoneNumber", txtPhoneNumber.Text},
                {"@AddressPatient", txtAddress.Text},
                {"@Email", txtEmail.Text},
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFindPatient_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT 
                                    bn.PatientID AS ""Mã bệnh nhân"", 
                                    bn.FullName AS ""Họ và tên"", 
                                    bn.DateOfBirth AS ""Ngày sinh"", 
                                    bn.Gender AS ""Giới tính"", 
                                    bn.PhoneNumber AS ""Số điện thoại"", 
                                    bn.AddressPatient AS ""Địa chỉ"", 
                                    bn.Email AS ""Email""
                                FROM 
                                    PATIENT AS bn
                                 WHERE 1=1";
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
            }
        }
    }


}

