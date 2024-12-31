using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;
using QuanLyBenhVien.Classes;

namespace QuanLyBenhVien
{
    public partial class BenhTheoThang : Form
    {
        public BenhTheoThang()
        {
            InitializeComponent();
            CommonControls.InitializelstDiagnosis(lstDiagnosis);
        }

        string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        private void btnOK_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            // Lấy tháng và năm từ TextBox
            int thang, nam;
            if (!int.TryParse(txtMonth.Text, out thang) || thang < 1 || thang > 12)
            {
                MessageBox.Show("Tháng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(txtYear.Text, out nam) || nam < 1)
            {
                MessageBox.Show("Năm không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Danh sách loại bệnh cần thống kê
            List<string> danhSachBenh = new List<string>();

            // Kiểm tra xem có mục nào được chọn không
            if (lstDiagnosis.SelectedItems.Count > 0)
            {
                // Lấy các mục đã chọn
                foreach (var item in lstDiagnosis.SelectedItems)
                {
                    danhSachBenh.Add(item.ToString());
                }
            }
            else
            {
                // Nếu không chọn mục nào, lấy tất cả các loại bệnh từ database
                danhSachBenh = LayTatCaLoaiBenh();
            }

            // Lặp qua danh sách bệnh và vẽ biểu đồ
            foreach (string loaiBenh in danhSachBenh)
            {
                DataTable dt = LayDuLieuLoaiBenh(loaiBenh, thang, nam);

                // Tạo Series cho mỗi loại bệnh
                Series series = new Series(loaiBenh)
                {
                    //ChartType = SeriesChartType.Line,
                    //BorderWidth = 5,

                     ChartType = SeriesChartType.Line,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 9,
                    BorderWidth = 3
                };

                //series.ChartType = SeriesChartType.Line;

                // Thêm dữ liệu vào Series
                foreach (DataRow row in dt.Rows)
                {
                    int ngay = Convert.ToInt32(row["Ngay"]);
                    int soLuong = Convert.ToInt32(row["SoLuongCaBenh"]);
                    series.Points.AddXY(ngay, soLuong);
                }

                // Thêm Series vào biểu đồ
                chart1.Series.Add(series);
            }

            // Thiết lập biểu đồ
            chart1.ChartAreas[0].AxisX.Title = "Ngày";
            chart1.ChartAreas[0].AxisY.Title = "Số lượng ca bệnh";
            chart1.Titles.Clear();
            chart1.Titles.Add("Thống kê số lượng ca bệnh theo ngày");



            // Thiết lập trục x và y
            chart1.ChartAreas[0].AxisX.Title = "Ngày";
            chart1.ChartAreas[0].AxisY.Title = "Số lượng ca bệnh";
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Segoe UI", 11F, FontStyle.Regular);
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 11F, FontStyle.Regular);

            chart1.Titles.Clear();
            chart1.Titles.Add("Thống kê số lượng ca bệnh theo ngày");

            if (chart1.Legends.Count > 0)
            {
                chart1.Legends[0].Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            }

            chart1.Titles[0].Font = new Font("Segoe UI", 14F, FontStyle.Bold);

        }

        private DataTable LayDuLieuLoaiBenh(string loaiBenh, int thang, int nam)
        {
            DataTable dt = new DataTable();




            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Truy vấn SQL lọc theo loại bệnh, tháng và năm
                string query = @"
                               SELECT DAY(VisitDate) AS Ngay, COUNT(*) AS SoLuongCaBenh
                               FROM MEDICALRECORD
                               WHERE Diagnosis = @LoaiBenh 
                               AND MONTH(VisitDate) = @Thang 
                               AND YEAR(VisitDate) = @Nam
                               GROUP BY DAY(VisitDate)
                               ORDER BY Ngay
                               ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoaiBenh", string.IsNullOrEmpty(loaiBenh) ? (object)DBNull.Value : loaiBenh);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }


        private List<string> LayTatCaLoaiBenh()
        {
            List<string> danhSachBenh = new List<string>();

              using (SqlConnection conn = new SqlConnection(connectionString))
              {
                string query = "SELECT DISTINCT Diagnosis FROM MEDICALRECORD";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                danhSachBenh.Add(reader["Diagnosis"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
              }

            return danhSachBenh;
        }
    }
}
