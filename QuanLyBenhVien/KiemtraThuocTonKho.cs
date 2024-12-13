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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBenhVien
{
    public partial class KiemtraThuocTonKho : Form
    {
        public KiemtraThuocTonKho()
        {
            InitializeComponent();
        }


        string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbCategory.Text == "" && cmbDosageUnit.Text == "" && txtQuantityInStock.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Câu truy vấn SQL
                        string query = $"SELECT MedicationID, MedicationName, QuantityInStock FROM MEDICATION  WHERE QuantityInStock <= {txtQuantityInStock.Text}";

                        // Tạo SqlDataAdapter để lấy dữ liệu
                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                        // Đổ dữ liệu vào DataTable
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Hiển thị dữ liệu lên DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Thông báo thành công
                        //  MessageBox.Show("Dữ liệu đã được tải thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            if (cmbCategory.Text == "" && cmbDosageUnit.Text != "" && txtQuantityInStock.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Câu truy vấn SQL
                        string query = $"SELECT MedicationID, MedicationName, QuantityInStock FROM MEDICATION WHERE DosageUnit = N'{cmbDosageUnit.Text}' AND QuantityInStock<= {txtQuantityInStock.Text}";

                        // Tạo SqlDataAdapter để lấy dữ liệu
                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                        // Đổ dữ liệu vào DataTable
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Hiển thị dữ liệu lên DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Thông báo thành công
                        //  MessageBox.Show("Dữ liệu đã được tải thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (cmbCategory.Text != "" && cmbDosageUnit.Text != "" && txtQuantityInStock.Text != "")
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Câu truy vấn SQL
                        string query = $"SELECT MedicationID, MedicationName, QuantityInStock FROM MEDICATION WHERE Category = N'{cmbCategory.Text}' AND DosageUnit = N'{cmbDosageUnit.Text}' AND QuantityInStock<= {txtQuantityInStock.Text}";

                        // Tạo SqlDataAdapter để lấy dữ liệu
                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

                        // Đổ dữ liệu vào DataTable
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Hiển thị dữ liệu lên DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Thông báo thành công
                        //  MessageBox.Show("Dữ liệu đã được tải thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void KiemtraThuocTonKho_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);

            // Chỉnh font cho nội dung các ô
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);

            // Chỉnh chiều cao của dòng
            dataGridView1.RowTemplate.Height = 30;
        }

       
    }
}
