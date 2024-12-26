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

namespace QuanLyBenhVien
{
    public partial class BenhTheoNam : Form
    {
        public BenhTheoNam()
        {
            InitializeComponent();
        }

        string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        private void btnOK_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            // Lấy danh sách các năm từ TextBox
            List<int> danhSachNam = txtYears.Text
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.TryParse(n.Trim(), out int year) ? year : (int?)null)
                .Where(year => year.HasValue).Select(year => year.Value).ToList();

            if (danhSachNam.Count == 0)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một năm hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Danh sách loại bệnh cần thống kê
            List<string> danhSachBenh = lstDiagnosis.SelectedItems.Count > 0
                ? lstDiagnosis.SelectedItems.Cast<string>().ToList()
                : LayTatCaLoaiBenh();

            // Truy xuất và vẽ dữ liệu
            foreach (string loaiBenh in danhSachBenh)
            {
                foreach (int nam in danhSachNam)
                {
                    DataTable dt = LayDuLieuTheoThang(loaiBenh, nam);

                    // Tạo Series cho từng loại bệnh và năm
                    string tenSeries = $"{loaiBenh} - {nam}";
                    Series series = new Series(tenSeries)
                    {
                        ChartType = SeriesChartType.Line,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = 9,
                        BorderWidth = 3
                    };

                    // Thêm dữ liệu vào Series
                    foreach (DataRow row in dt.Rows)
                    {
                        int thang = Convert.ToInt32(row["Thang"]);
                        int soLuong = Convert.ToInt32(row["SoLuongCaBenh"]);
                        series.Points.AddXY(thang, soLuong);
                    }

                    // Thêm Series vào biểu đồ
                    chart1.Series.Add(series);
                }
            }

            // Thiết lập biểu đồ
            chart1.ChartAreas[0].AxisX.Title = "Tháng";
            chart1.ChartAreas[0].AxisY.Title = "Số lượng ca bệnh";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.Titles.Clear();
            chart1.Titles.Add("Thống kê số lượng ca bệnh theo tháng và năm");
        }

        private DataTable LayDuLieuTheoThang(string loaiBenh, int nam)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                                 SELECT MONTH(VisitDate) AS Thang, COUNT(*) AS SoLuongCaBenh
                                 FROM MEDICALRECORD
                                 WHERE (@LoaiBenh IS NULL OR Diagnosis LIKE @LoaiBenh)
                                 AND YEAR(VisitDate) = @Nam
                                 GROUP BY MONTH(VisitDate)
                                 ORDER BY MONTH(VisitDate);
                                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoaiBenh", string.IsNullOrEmpty(loaiBenh) ? (object)DBNull.Value : $"%{loaiBenh.Trim()}%");
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
