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
    public partial class MedicineForm : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        public MedicineForm()
        {
            InitializeComponent();
        }

        private void MedicineForm_Load(object sender, EventArgs e)
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
                    dgvMedicine.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
                }
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(txtMedicationID.Text) || string.IsNullOrEmpty(txtMedicationName.Text) ||
               string.IsNullOrEmpty(txtDosage.Text) || string.IsNullOrEmpty(cmbCategory.Text) ||
               string.IsNullOrEmpty(txtQuantityInStock.Text) || string.IsNullOrEmpty(txtPrice.Text) ||
               string.IsNullOrEmpty(cmbDosageUnit.Text))
            {
                return false; 
            }

            if (!CommonChecks.HasDigit(txtPrice.Text) || !CommonChecks.HasDigit(txtQuantityInStock.Text))
            {
                return false;
            }    

            return true;
        }

        private void btnAddOrUpdateMedicine_Click(object sender, EventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin.");
                return;
            }

            string query = @"IF EXISTS (SELECT 1 FROM MEDICATION WHERE MedicationID = @MedicationID)
                     UPDATE MEDICATION 
                     SET MedicationName = @MedicationName, Dosage = @Dosage, DosageUnit = @DosageUnit, Category = @Category, 
                         QuantityInStock = @QuantityInStock, Price = @Price, 
                         ExpiryDate = @ExpiryDate, ManufacturingDate = @ManufacturingDate, Manufacturer = @Manufacturer
                     WHERE MedicationID = @MedicationID
                     ELSE
                     INSERT INTO MEDICATION (MedicationID, MedicationName, Dosage, DosageUnit, Category, QuantityInStock, Price, ExpiryDate, ManufacturingDate, Manufacturer)
                     VALUES (@MedicationID, @MedicationName, @Dosage, @DosageUnit, @Category, @QuantityInStock, @Price, @ExpiryDate, @ManufacturingDate, @Manufacturer)";

            var parameters = new Dictionary<string, object>
            {
                { "@MedicationID", txtMedicationID.Text},
                { "@MedicationName", txtMedicationName.Text},
                { "@Dosage", txtDosage.Text},
                { "@Category", cmbCategory.Text},
                { "@QuantityInStock",(txtQuantityInStock.Text)},
                { "@Price",(txtPrice.Text) },
                { "@ExpiryDate", dtpExpiryDate.Value.Date },
                { "@ManufacturingDate", dtpManufacturingDate.Value.Date },
                { "@Manufacturer", txtManufacturer.Text },
                {"@DosageUnit", cmbDosageUnit.Text}
            };

            CommonQuery.ExecuteQuery(query, parameters);
            LoadData();
            CommonControls.ResetInputFields(Parent);
        }

        private void btnFindMedicine_Click(object sender, EventArgs e)
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

                    if (!string.IsNullOrEmpty(txtMedicationID.Text))
                    {
                        query += " AND MedicationID = @MedicationID";
                        parameters.Add("@MedicationID", txtMedicationID.Text);
                    }
                    if (!string.IsNullOrEmpty(txtMedicationName.Text))
                    {
                        query += " AND MedicationName LIKE @MedicationName";
                        parameters.Add("@MedicationName", $"%{txtMedicationName.Text.Trim()}%");
                    }
                    if (!string.IsNullOrEmpty(cmbCategory.Text))
                    {
                        query += " AND Category LIKE @Category";
                        parameters.Add("@Category", $"%{cmbCategory.Text.Trim()}%");
                    }
                    if (!string.IsNullOrEmpty(cmbDosageUnit.Text))
                    {
                        query += " AND DosageUnit = @DosageUnit";
                        parameters.Add("@DosageUnit", cmbDosageUnit.Text);
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
        private void btnRemoveMedication_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM MEDICATION WHERE MedicationID = @MedicationID";
            var parameters = new Dictionary<string, object> { { "@MedicationID", txtMedicationID.Text } };
            CommonQuery.ExecuteQuery(query, parameters);
            CommonControls.ResetInputFields(Parent);
            LoadData();
        }

        private void dgvMedicine_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMedicine.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvMedicine.SelectedRows[0];
                txtMedicationID.Text = selectedRow.Cells[0].Value.ToString();
                txtMedicationName.Text = selectedRow.Cells[1].Value.ToString();
                txtDosage.Text = selectedRow.Cells[2].Value.ToString();
                cmbDosageUnit.Text = selectedRow.Cells[3].Value.ToString();
                cmbCategory.Text = selectedRow.Cells[4].Value.ToString();
                txtQuantityInStock.Text = selectedRow.Cells[5].Value.ToString();
                txtPrice.Text = selectedRow.Cells[6].Value.ToString();
                dtpExpiryDate.Text = selectedRow.Cells[7].Value.ToString();
                dtpManufacturingDate.Text = selectedRow.Cells[8].Value.ToString();
                txtManufacturer.Text = selectedRow.Cells[9].Value.ToString();
            }
        }

        private void btnRefreshMedication_Click(object sender, EventArgs e)
        {
            CommonControls.ResetInputFields(Parent);
        }
    }
}
