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
    public partial class NormalMedicine : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        string userID = null;
        public NormalMedicine()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT 
                                thuoc.MedicationID AS ""Mã thuốc"", 
                                thuoc.MedicationName AS ""Tên thuốc"", 
                                thuoc.Dosage AS ""Liều lượng"",
                                thuoc.DosageUnit AS ""Đơn vị tính"",
                                thuoc.Category AS ""Loại thuốc"", 
                                thuoc.QuantityInStock AS ""Số lượng tồn kho"", 
                                thuoc.Price AS ""Giá"", 
                                thuoc.ExpiryDate AS ""Ngày hết hạn"", 
                                thuoc.ManufacturingDate AS ""Ngày sản xuất"", 
                                thuoc.Manufacturer AS ""Nhà sản xuất""
                            FROM 
                                MEDICATION AS thuoc;
                            ";
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset, "MEDICATION");
                    dgvMedicine.DataSource = dataset.Tables["MEDICATION"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT 
                        thuoc.MedicationID AS ""Mã thuốc"", 
                        thuoc.MedicationName AS ""Tên thuốc"", 
                        thuoc.Dosage AS ""Liều lượng"", 
                        thuoc.DosageUnit AS ""Đơn vị tính"",
                        thuoc.Category AS ""Loại thuốc"", 
                        thuoc.QuantityInStock AS ""Số lượng tồn kho"", 
                        thuoc.Price AS ""Giá"", 
                        thuoc.ExpiryDate AS ""Ngày hết hạn"", 
                        thuoc.ManufacturingDate AS ""Ngày sản xuất"", 
                        thuoc.Manufacturer AS ""Nhà sản xuất""
                    FROM 
                        MEDICATION AS thuoc
                     WHERE 1=1";
                    var parameters = new Dictionary<string, object>();
                    if (!string.IsNullOrEmpty(txtMedicineName.Text))
                    {
                        query += " AND MedicationName LIKE @MedicationName";
                        parameters.Add("@MedicationName", $"%{txtMedicineName.Text.Trim()}%");
                    }
                    if (!string.IsNullOrEmpty(cmbCategory.Text))
                    {
                        query += " AND Category LIKE @Category";
                        parameters.Add("@Category", $"%{cmbCategory.Text.Trim()}%");
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

                        dgvMedicine.DataSource = resultTable;

                        if (resultTable.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy thuốc phù hợp với điều kiện tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
