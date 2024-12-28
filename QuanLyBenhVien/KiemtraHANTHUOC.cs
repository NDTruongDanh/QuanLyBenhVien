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
    public partial class KiemtraHANTHUOC : Form
    {
        public KiemtraHANTHUOC()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHanThuoc.Text))
            {
                MessageBox.Show("Vui lòng nhập số ngày cần kiểm tra!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển đổi giá trị TextBox thành số nguyên
            int soNgay;
            if (!int.TryParse(txtHanThuoc.Text, out soNgay) || soNgay <= 0)
            {
                MessageBox.Show("Số ngày nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tính ngày kiểm tra (ngày hiện tại + N ngày)
            DateTime ngayCanKiemTra = DateTime.Now.AddDays(soNgay);

            DateTime now = DateTime.Now;
            string ngayHientai = now.ToString("yyyy-MM-dd");
            // Kết nối SQL Server và truy vấn dữ liệu
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Câu truy vấn SQL
                    string query = $"SELECT MedicationID AS [Mã thuốc], MedicationName AS [Tên thuốc], ExpiryDate AS[Ngày hết hạn] FROM MEDICATION WHERE ExpiryDate >= @ngayHientai AND ExpiryDate <= @NgayCanKiemTra";

                    // Thực thi truy vấn
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        // Thêm tham số cho câu lệnh SQL
                        adapter.SelectCommand.Parameters.AddWithValue("@NgayCanKiemTra", ngayCanKiemTra);
                        adapter.SelectCommand.Parameters.AddWithValue("@ngayHientai", ngayHientai);

                        // Đổ dữ liệu vào DataTable
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Hiển thị dữ liệu lên DataGridView
                        dgvExpireInfo.DataSource = dt;
                        dgvExpireInfo.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

                        // Thông báo
                        //   MessageBox.Show($"Đã tải danh sách các thuốc còn hạn trong {soNgay} ngày.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void KiemtraHANTHUOC_Load(object sender, EventArgs e)
        {
            dgvExpireInfo.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);

            // Chỉnh font cho nội dung các ô
            dgvExpireInfo.DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Regular);

            // Chỉnh chiều cao của dòng
            dgvExpireInfo.RowTemplate.Height = 30;
        }

        private void btnExpired_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Câu truy vấn SQL
                    string query = $"SELECT MedicationID AS [Mã thuốc], MedicationName AS [Tên thuốc], ExpiryDate AS [Ngày hết hạn] FROM MEDICATION WHERE ExpiryDate < @ngayHientai";

                    DateTime now = DateTime.Now;
                    string ngayHientai = now.ToString("yyyy-MM-dd");
                    // Thực thi truy vấn
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        // Thêm tham số cho câu lệnh SQL
                       
                        adapter.SelectCommand.Parameters.AddWithValue("@ngayHientai", ngayHientai);

                        // Đổ dữ liệu vào DataTable
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Hiển thị dữ liệu lên DataGridView
                        dgvExpireInfo.DataSource = dt;

                        // Thông báo
                        //   MessageBox.Show($"Đã tải danh sách các thuốc còn hạn trong {soNgay} ngày.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
