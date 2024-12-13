using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyBenhVien
{
    public partial class DoanhthuTHANG : Form
    {
        public DoanhthuTHANG()
        {
            InitializeComponent();
        }


        private void LoadDoanhThuChart(int thang, int nam)
        {
            // Chuỗi kết nối SQL
            string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
            string query = $@"SELECT TransactionDate AS Ngay, SUM(Total) AS DoanhThu,(SELECT SUM(Total) 
                                                                                      FROM BILL 
                                                                                      WHERE MONTH(TransactionDate) = {thang} AND YEAR(TransactionDate) = {nam})
                                                                                        AS TongDoanhThu
                            FROM BILL
                            WHERE MONTH(TransactionDate) = {thang}   AND YEAR(TransactionDate)= {nam}
                            GROUP BY TransactionDate";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            double tongDoanhThu = 0;

            // Gán dữ liệu vào Chart
            chart1.Series.Clear();
            chart1.Series.Add("DoanhThu");
            chart1.Series["DoanhThu"].ChartType = SeriesChartType.Line;
          

            foreach (DataRow row in dt.Rows)
            {
                DateTime ngay = Convert.ToDateTime(row["Ngay"]); // Chuyển cột Ngay thành DateTime
                string ngayHienThi = ngay.Day.ToString(); // Lấy ngày (1-31)

                chart1.Series["DoanhThu"].Points.AddXY(ngayHienThi, row["DoanhThu"]); 

                tongDoanhThu = Convert.ToDouble(row["TongDoanhThu"]);
            }

            chart1.ChartAreas[0].AxisX.Title = "Ngày";
            chart1.ChartAreas[0].AxisY.Title = "Doanh Thu (VND)";
            lblTotal.Text = $"Tổng Doanh Thu Tháng {thang}/{nam}: {tongDoanhThu:N0} VND";


        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            int thang = int.Parse(txtMonth.Text); // Tháng nhập từ TextBox
            int nam = int.Parse(txtYear.Text);     // Năm nhập từ TextBox
            LoadDoanhThuChart(thang, nam);
        }
    }
}
