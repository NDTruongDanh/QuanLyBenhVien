using QuanLyBenhVien.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class PatientCare : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        string userID = null;
        public PatientCare(string userID)
        {
            InitializeComponent();
            CommonControls.InitializeCmbRoomID(cmbRoomID);
            this.userID = userID;
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = $@"SELECT CareID AS[Mã chăm sóc], FullName AS [Tên bệnh nhân], RoomID AS [Phòng], CareDateTime AS [Ngày bắt đầu chăm sóc], CareType AS [Loại chăm sóc], Notes AS [Ghi chú]
                                    FROM NURSECARE n JOIN PATIENT p ON n.PatientID = p.PatientID
                                    WHERE NurseID = '{userID}'";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, conn))
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            dataAdapter.Fill(dataSet);
                            if (dataSet.Tables.Count > 0)
                            {
                                dgvPatientCare.DataSource = dataSet.Tables[0];
                                dgvPatientCare.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                            }
                            else
                                MessageBox.Show("Không có bệnh nhân!");
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void dgvPatientCare_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPatientCare.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPatientCare.SelectedRows[0];
                lblCareID.Text = row.Cells[0].Value.ToString();
                txtPatientName.Text = row.Cells[1].Value.ToString();
                cmbRoomID.Text = row.Cells[2].Value.ToString();
                txtTypeOfCare.Text = row.Cells[4].Value.ToString();
                txtNotes.Text = row.Cells[5].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = $@"UPDATE NURSECARE
                                    SET Notes = N'{txtNotes.Text}', RoomID = '{cmbRoomID.Text}', CareType = N'{txtTypeOfCare.Text}'
                                    WHERE CareID = '{lblCareID.Text}'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!");
                            LoadData();
                            CommonControls.ResetInputFields(Parent);
                        }
                        else
                            MessageBox.Show("Cập nhật thất bại");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT CareID AS [Mã chăm sóc], 
                                  FullName AS [Tên bệnh nhân], 
                                  RoomID AS [Phòng], 
                                  CareDateTime AS [Ngày bắt đầu chăm sóc], 
                                  CareType AS [Loại chăm sóc], 
                                  Notes AS [Ghi chú]
                           FROM NURSECARE n
                           JOIN PATIENT p ON n.PatientID = p.PatientID
                           WHERE NurseID = @NurseID";

                    var command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@NurseID", userID);

                    if (!string.IsNullOrEmpty(txtPatientName.Text))
                    {
                        sql += " AND FullName LIKE @FullName";
                        command.Parameters.AddWithValue("@FullName", $"%{txtPatientName.Text.Trim()}%");
                    }

                    if (!string.IsNullOrEmpty(cmbRoomID.Text))
                    {
                        sql += " AND RoomID = @RoomID";
                        command.Parameters.AddWithValue("@RoomID", cmbRoomID.Text);
                    }
                    if (!string.IsNullOrEmpty(txtTypeOfCare.Text))
                    {
                        sql += " AND CareType = @CareType";
                        command.Parameters.AddWithValue("@CareType", txtTypeOfCare.Text);
                    }


                    command.CommandText = sql;

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        using (DataSet dataSet = new DataSet())
                        {
                            dataAdapter.Fill(dataSet);
                            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                            {
                                dgvPatientCare.DataSource = dataSet.Tables[0];
                            }
                            else
                            {
                                MessageBox.Show("Không có bệnh nhân phù hợp!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
