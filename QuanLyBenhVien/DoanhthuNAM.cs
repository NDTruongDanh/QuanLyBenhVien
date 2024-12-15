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
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBenhVien
{
    public partial class DoanhthuNAM : Form
    {
        public DoanhthuNAM()
        {
            InitializeComponent();
           
        }

        private void DoanhthuNAM_Load(object sender, EventArgs e)
        {
            LoadChartData(null);
            chartMonth.Series.Clear();
            chartYear.Series.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string inputYears = txtYear.Text; // Ví dụ: "2022,2023,2024"
            if (!string.IsNullOrWhiteSpace(inputYears))
            {
                // Chuyển chuỗi thành mảng năm
                var years = inputYears.Split(',')
                                      .Select(y => y.Trim())
                                      .Where(y => int.TryParse(y, out _)) // Lọc năm hợp lệ
                                      .ToArray();

                if (years.Length > 0)
                {
                    // Tải biểu đồ với các năm cụ thể
                    LoadChartData(years);
                }
                else
                {

                    MessageBox.Show("Vui lòng nhập năm hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else { LoadChartData(null); }
        }

        private void LoadChartData(string[] years)
        {
            // Chuỗi kết nối SQL Server
            string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

            // Query lọc dữ liệu
            string query = @"
                        SELECT YEAR(TransactionDate) AS Year,
                        MONTH(TransactionDate) AS Month,
                        SUM(Total) AS TotalAmount
                        FROM BILL
                        GROUP BY YEAR(TransactionDate), MONTH(TransactionDate)";

            if (years != null && years.Length > 0)
            {
                // Thêm điều kiện WHERE để lọc năm
                string yearCondition = string.Join(",", years);
                query += $" HAVING YEAR(TransactionDate) IN ({yearCondition})";
            }

            // Lấy dữ liệu từ SQL
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dataTable);
            }
            DataView dataView = dataTable.DefaultView;
            dataView.Sort = "Year ASC";
            DataTable sortedTable = dataView.ToTable();

            // Hiển thị dữ liệu trên biểu đồ
            DisplayChart(sortedTable);

            DisplayYearTotalChart(sortedTable);
        }

        private void DisplayChart(DataTable dataTable)
        {
            chartMonth.Series.Clear(); // Xóa dữ liệu cũ

            // Tạo Series cho từng năm
            var years = dataTable.DefaultView.ToTable(true, "Year");
            foreach (DataRow yearRow in years.Rows)
            {
                string year = yearRow["Year"].ToString();
                Series series = new Series(year)
                {
                    ChartType = SeriesChartType.Line,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 9,
                    BorderWidth = 3
                };

                // Thêm điểm vào Series
                DataRow[] rows = dataTable.Select($"Year = {year}");
                foreach (DataRow row in rows)
                {
                    int month = Convert.ToInt32(row["Month"]);
                    decimal total = Convert.ToDecimal(row["TotalAmount"]);
                    series.Points.AddXY(month, total);
                }

                chartMonth.Series.Add(series);
            }

            // Cấu hình trục biểu đồ
            chartMonth.ChartAreas[0].AxisX.Title = "Tháng";
            chartMonth.ChartAreas[0].AxisY.Title = "Tổng số tiền";
            chartMonth.ChartAreas[0].AxisX.Minimum = 1;
            chartMonth.ChartAreas[0].AxisX.Maximum = 12;

            chartMonth.Titles.Clear();
            chartMonth.Titles.Add("Biểu đồ đường - Tổng tiền theo tháng của từng năm");
        }
        private void DisplayYearTotalChart(DataTable dataTable)
        {
            chartYear.Series.Clear(); // Xóa dữ liệu cũ
            chartYear.Titles.Clear();

            // Thêm tiêu đề
            chartYear.Titles.Add("Tổng tiền từng năm");

            // Tạo Series biểu đồ cột
            Series yearTotalSeries = new Series("Tổng tiền")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.Orange,
                // IsValueShownAsLabel = true // Hiển thị giá trị tổng tiền trên cột
            };

            // Tính tổng tiền từng năm
            var yearGroups = dataTable.AsEnumerable()
                                      .GroupBy(row => row.Field<int>("Year"))
                                      .Select(g => new
                                      {
                                          Year = g.Key,
                                          Total = g.Sum(row => row.Field<decimal>("TotalAmount"))
                                      });

            // Thêm dữ liệu tổng tiền từng năm vào Series
            foreach (var yearGroup in yearGroups)
            {
                yearTotalSeries.Points.AddXY($"Năm {yearGroup.Year}", yearGroup.Total);
            }

            chartYear.Series.Add(yearTotalSeries);

            // Cấu hình biểu đồ con
            chartYear.ChartAreas[0].AxisX.Title = "Năm";
            chartYear.ChartAreas[0].AxisY.Title = "Tổng số tiền (VND)";
            chartYear.ChartAreas[0].AxisX.Interval = 1;
        }
    }
}
