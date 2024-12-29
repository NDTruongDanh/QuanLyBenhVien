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
    public partial class RoomForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        public RoomForm()
        {
            InitializeComponent();
            CommonControls.InitializeCmbDepartmentID(cmbDepartmentID);
            CommonControls.InitializeCmbTypeOfRoom(cmbRoomType);
        }

        private void RoomForm_Load(object sender, EventArgs e)
        {
            LoadRoomData();
        }


        private void LoadRoomData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT 
                                    phong.RoomID AS ""Mã phòng"", 
                                    phong.DepartmentID AS ""Mã khoa"", 
                                    phong.BedCount AS ""Số giường"", 
                                    phong.RoomType AS ""Loại phòng"",
                                    phong.EmptyBed AS ""Giường trống""
                                FROM 
                                    ROOM AS phong;
                                ";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "ROOM");
                    dgvRoom.DataSource = dataSet.Tables["ROOM"];
                    dgvRoom.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
            }
        }


        private bool IsValid()
        {
            if (string.IsNullOrEmpty(txtRoomID.Text) || string.IsNullOrEmpty(txtBedCount.Text) ||
                string.IsNullOrEmpty(cmbRoomType.Text) || !CommonChecks.IsNumber(txtBedCount.Text))
            {
                return false;
            }
            return true;
        }


        private void btnAddOrUpdateRoom_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM ROOM WHERE RoomID = @RoomID)
                     UPDATE ROOM 
                     SET DepartmentID = @DepartmentID, BedCount = @BedCount, RoomType = @RoomType, EmptyBed = @EmptyBed 
                     WHERE RoomID = @RoomID
                     ELSE
                     INSERT INTO ROOM (RoomID, DepartmentID, BedCount, RoomType, EmptyBed) 
                     VALUES (@RoomID, @DepartmentID, @BedCount, @RoomType, @EmptyBed)";

            var parameters = new Dictionary<string, object>
            {
                { "@RoomID", txtRoomID.Text.Trim() },
                { "@DepartmentID",cmbDepartmentID.Text.Trim() },
                { "@BedCount", int.Parse(txtBedCount.Text.Trim()) },
                { "@RoomType", cmbRoomType.Text.Trim() },
                { "@EmptyBed", int.Parse(txtEmptyBed.Text.Trim()) },
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadRoomData();
            CommonControls.ResetInputFields(Parent);
        }

     



        private void btnFindRoom_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT 
                                    phong.RoomID AS ""Mã phòng"", 
                                    phong.DepartmentID AS ""Mã khoa"", 
                                    phong.BedCount AS ""Số giường"", 
                                    phong.RoomType AS ""Loại phòng""
                                    phong.EmptyBed AS ""Giường trống""
                                FROM 
                                    ROOM AS phong
                                 WHERE 1=1";
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(txtRoomID.Text))
                    {
                        query += " AND RoomID = @RoomID";
                        parameters.Add("@RoomID", txtRoomID.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(cmbDepartmentID.Text))
                    {
                        query += " AND DepartmentID = @DepartmentID";
                        parameters.Add("@DepartmentID", cmbDepartmentID.Text.Trim());
                    }
                    if (!string.IsNullOrEmpty(cmbRoomType.Text))
                    {
                        query += " AND RoomType LIKE @RoomType";
                        parameters.Add("@RoomType", $"%{cmbRoomType.Text.Trim()}%");
                    }
                    
                    if (int.TryParse(txtEmptyBed.Text, out int result))
                    {
                        query += " AND EmptyBed = @EmptyBed";
                        parameters.Add("@EmptyBed", result);
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

                        dgvRoom.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy phòng phù hợp với điều kiện tìm kiếm.",
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

        private void btnRemoveRoom_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM ROOM WHERE RoomID = @RoomID";
            var parameters = new Dictionary<string, object> { { "@RoomID", txtRoomID.Text } };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadRoomData();
        }

        private void btnRefreshRoom_Click(object sender, EventArgs e)
        {
            CommonControls.ResetInputFields(Parent);
        }

        private void dgvRoom_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRoom.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvRoom.SelectedRows[0];

                txtRoomID.Text = selectedRow.Cells[0].Value.ToString();
                cmbDepartmentID.Text = selectedRow.Cells[1].Value.ToString();
                txtBedCount.Text = selectedRow.Cells[2].Value.ToString();
                cmbRoomType.Text = selectedRow.Cells[3].Value.ToString();
                txtEmptyBed.Text = selectedRow.Cells[4].Value.ToString();
            }
        }
    }
}