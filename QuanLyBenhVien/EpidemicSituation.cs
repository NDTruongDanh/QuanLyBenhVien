using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class EpidemicSituation : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        public EpidemicSituation()
        {
            InitializeComponent();
        }

        // Định nghĩa lớp MedicalRecord để lưu trữ thông tin bệnh nhân
        public class MedicalRecord
        {
            public DateTime VisitDate { get; set; } // Ngày thăm khám
            public string Diagnosis { get; set; }   // Chẩn đoán bệnh
        }

        // Định nghĩa lớp ComparisonRecord để lưu trữ thông tin so sánh
        public class ComparisonRecord
        {
            public string Diagnosis { get; set; }  // Chẩn đoán bệnh
            public DateTime VisitDate { get; set; } // Ngày thăm khám
            public int Week { get; set; }          // Tuần của năm
            public int Month { get; set; }         // Tháng của năm
            public int Year { get; set; }          // Năm
            public int CaseCount { get; set; }     // Số ca bệnh
        }

        // Định nghĩa lớp ChangeRecord để lưu trữ thông tin thay đổi
        public class ChangeRecord
        {
            public string Diagnosis { get; set; }
            public DateTime VisitDate { get; set; }
            public int Week { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public int CaseCount { get; set; }
            public double ChangeRate { get; set; }  // Tỷ lệ thay đổi
        }

        // Hàm lấy ISO Week của năm từ một ngày cụ thể
        public static int GetISOWeekOfYear(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3); // Điều chỉnh nếu ngày là từ thứ Hai đến thứ Tư
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        // Hàm lấy ngày đầu tiên của tuần ISO
        public static DateTime GetDateFromISOWeek(int year, int week)
        {
            var jan1 = new DateTime(year, 1, 1);
            var days = (week - 1) * 7 - (int)jan1.DayOfWeek + (int)DayOfWeek.Monday;
            return jan1.AddDays(days);
        }



        private List<MedicalRecord> GetMedicalRecords()
        {
            List<MedicalRecord> records = new List<MedicalRecord>();
            string query = null;

            DateTime current = DateTime.Now;

            DateTime timePeriod;

            // Lọc bệnh theo tuần hoặc theo tháng dựa trên lựa chọn của người dùng
            if (cmbTimePeriod.Text == "Theo tuần")
            {
                timePeriod = current.AddDays(-14);  // Lọc trong vòng 14 ngày
                query = "SELECT Diagnosis, VisitDate FROM MEDICALRECORD WHERE Diagnosis IN (N'Cảm cúm','Covid-19',N'Viêm phổi') AND VisitDate >= @Date";
            }
            else
            {
                timePeriod = current.AddMonths(-2); // Lọc trong vòng 2 tháng
                query = "SELECT Diagnosis, VisitDate FROM MEDICALRECORD WHERE Diagnosis IN (N'Sốt xuất huyết',N'Viêm phổi',N'Tiêu chảy do nhiễm khuẩn') AND VisitDate >= @Date";
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", timePeriod);  // Lọc theo ngày

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MedicalRecord record = new MedicalRecord
                                {
                                    VisitDate = Convert.ToDateTime(reader["VisitDate"]), // Chuyển đổi ngày thăm khám
                                    Diagnosis = reader["Diagnosis"].ToString() // Lấy chẩn đoán bệnh
                                };
                                records.Add(record); // Thêm hồ sơ vào danh sách
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            return records;
        }


        // Hàm thông báo khi có sự gia tăng đột ngột về số ca bệnh
        private void NotifySuddenIncrease(List<ComparisonRecord> comparison, double threshold = 50)
        {
            var groupedByDiagnosis = comparison
                .GroupBy(c => c.Diagnosis) // Nhóm theo chẩn đoán bệnh
                .SelectMany(g =>
                {
                    var diagnosisRecords = g.OrderBy(r => r.Year).ThenBy(r => r.Month).ToList();

                    return diagnosisRecords.Select((current, index) =>
                    {
                        if (index == 0)
                            return null; // Không có bản ghi trước để so sánh

                        var previous = diagnosisRecords[index - 1]; // Lấy bản ghi trước

                        // Tính tỷ lệ thay đổi giữa số ca bệnh hiện tại và trước đó
                        double changeRate = previous.CaseCount == 0 ? (current.CaseCount > 0 ? 100 : 0) : ((current.CaseCount - previous.CaseCount) / (double)previous.CaseCount) * 100;

                        // Nếu tỷ lệ thay đổi vượt ngưỡng, thông báo
                        if (changeRate >= threshold)
                        {
                            return new
                            {
                                Diagnosis = current.Diagnosis,
                                previousCaseCount = previous.CaseCount,
                                currentCaseCount = current.CaseCount,
                                PreviousPeriod = previous.Week == 0 ? previous.Month + "-" + previous.Year : GetDateFromISOWeek(previous.Year, previous.Week).ToString("dd-MM-yyyy"),
                                CurrentPeriod = current.Week == 0 ? current.Month + "-" + current.Year : GetDateFromISOWeek(current.Year, current.Week).ToString("dd-MM-yyyy"),
                                ChangeRate = changeRate
                            };
                        }

                        return null;
                    }).Where(x => x != null).ToList(); // Lọc các bản ghi có thay đổi
                }).ToList();


            // Nếu có sự gia tăng đột ngột, hiển thị thông tin
            if (groupedByDiagnosis.Any())
            {
                StringBuilder notificationMessage = new StringBuilder();
                foreach (var record in groupedByDiagnosis)
                {
                    notificationMessage.AppendLine($"Loại bệnh: {record.Diagnosis}\n" +
                                    $"Số ca mắc bệnh tăng từ: {record.previousCaseCount} ({record.PreviousPeriod}) đến {record.currentCaseCount} ({record.CurrentPeriod})\n" +
                                    $"Tỷ lệ gia tăng: {record.ChangeRate:F2}%\n\n");
                }

                foreach (var record in groupedByDiagnosis)
                {

                    // Thêm thông báo vào RichTextBox
                    // Loại bệnh
                    AppendFormattedText("Loại bệnh: ", Color.Black, FontStyle.Bold);
                    AppendFormattedText($"{record.Diagnosis}\n", Color.Red, FontStyle.Bold);

                    // Số ca bệnh
                    AppendFormattedText("Số ca mắc bệnh tăng từ: ", Color.Black, FontStyle.Regular);
                    AppendFormattedText($"{record.previousCaseCount} ({record.PreviousPeriod}) ", Color.Blue, FontStyle.Regular);
                    AppendFormattedText($"đến {record.currentCaseCount} ({record.CurrentPeriod})\n", Color.Green, FontStyle.Regular);

                    // Tỷ lệ gia tăng
                    AppendFormattedText("Tỷ lệ gia tăng: ", Color.Black, FontStyle.Regular);
                    AppendFormattedText($"{record.ChangeRate:F2}%\n\n", Color.OrangeRed, FontStyle.Bold);
                }
            }
            else
            {
              rtxtEpimedic.AppendText("Không phát hiện sự gia tăng bất thường!");
            }

            
        }

        // Hàm thêm văn bản có định dạng vào RichTextBox
        private void AppendFormattedText(string text, Color color, FontStyle fontStyle)
        {
            if (rtxtEpimedic.InvokeRequired)
            {
                rtxtEpimedic.Invoke(new Action(() => AppendFormattedText(text, color, fontStyle)));
            }
            else
            {
                rtxtEpimedic.SelectionStart = rtxtEpimedic.TextLength;
                rtxtEpimedic.SelectionLength = 0;
                rtxtEpimedic.SelectionColor = color;
                rtxtEpimedic.SelectionFont = new Font(rtxtEpimedic.Font, fontStyle);
                rtxtEpimedic.AppendText(text);
                rtxtEpimedic.SelectionColor = rtxtEpimedic.ForeColor;

            }
        }

        // Hàm tính tỷ lệ thay đổi giữa các bản ghi
        private List<ChangeRecord> CalculateChangeRates(List<ComparisonRecord> comparison)
        {
            // Sử dụng LINQ để tính toán tỷ lệ thay đổi cho từng bản ghi trong danh sách `comparison`.
            return comparison.Select((current, index) =>
            {
                // Nếu là bản ghi đầu tiên trong danh sách, không có bản ghi trước đó để so sánh, tỷ lệ thay đổi = 0
                if (index == 0)
                    return new ChangeRecord
                    {
                        Diagnosis = current.Diagnosis,        // Chẩn đoán bệnh
                        VisitDate = current.VisitDate,        // Ngày khám
                        Week = current.Week,                  // Tuần (nếu có)
                        Month = current.Month,                // Tháng (nếu có)
                        Year = current.Year,                  // Năm
                        CaseCount = current.CaseCount,        // Số ca bệnh
                        ChangeRate = 0                        // Tỷ lệ thay đổi = 0 cho bản ghi đầu tiên
                    };

                // Nếu không phải bản ghi đầu tiên, tính toán tỷ lệ thay đổi giữa bản ghi hiện tại và bản ghi trước đó
                var previous = comparison[index - 1];    // Lấy bản ghi trước đó để so sánh
                return new ChangeRecord
                {
                    Diagnosis = current.Diagnosis,        // Chẩn đoán bệnh
                    VisitDate = current.VisitDate,        // Ngày khám
                    Week = current.Week,                  // Tuần (nếu có)
                    Month = current.Month,                // Tháng (nếu có)
                    Year = current.Year,                  // Năm
                    CaseCount = current.CaseCount,        // Số ca bệnh
                                                          // Tính tỷ lệ thay đổi giữa số ca bệnh hiện tại và số ca bệnh của bản ghi trước đó
                    ChangeRate = previous.CaseCount == 0   // Kiểm tra tránh chia cho 0
                        ? 0                                // Nếu số ca của bản ghi trước là 0, tỷ lệ thay đổi = 0
                        : ((current.CaseCount - previous.CaseCount) / (double)previous.CaseCount) * 100   // Công thức tính tỷ lệ thay đổi
                };
            }).ToList();  // Chuyển đổi kết quả của LINQ thành danh sách `List<ChangeRecord>`
        }



        // Hàm lấy các bản ghi so sánh theo tuần hoặc theo tháng





        private List<ComparisonRecord> GetComparisonRecords(List<MedicalRecord> records, string period)
        {
            // Kiểm tra nếu khoảng thời gian chọn là "Theo tuần"
            if (period == "Theo tuần")
            {
                // Nhóm các bản ghi theo chẩn đoán bệnh, tuần trong năm (dùng hàm GetISOWeekOfYear để tính tuần)
                return records
                    .GroupBy(r => new { r.Diagnosis, Week = GetISOWeekOfYear(r.VisitDate), r.VisitDate.Year })  // Nhóm theo chẩn đoán bệnh, tuần và năm
                    .Select(g => new ComparisonRecord
                    {
                        Diagnosis = g.Key.Diagnosis,       // Chẩn đoán bệnh
                        Week = g.Key.Week,                 // Tuần trong năm
                        Year = g.Key.Year,                 // Năm
                        CaseCount = g.Count()              // Số lượng bản ghi trong nhóm (tức là số ca bệnh cho tuần đó)
                    })
                    .OrderBy(x => x.Year)                  // Sắp xếp theo năm
                    .ThenBy(x => x.Week)                   // Sắp xếp theo tuần
                    .ToList();                            // Chuyển đổi kết quả thành danh sách List<ComparisonRecord>
            }
            else
            {
                // Nếu khoảng thời gian chọn là "Theo tháng"
                // Nhóm các bản ghi theo chẩn đoán bệnh, tháng và năm
                return records
                    .GroupBy(r => new { r.Diagnosis, r.VisitDate.Month, r.VisitDate.Year })  // Nhóm theo chẩn đoán bệnh, tháng và năm
                    .Select(g => new ComparisonRecord
                    {
                        Diagnosis = g.Key.Diagnosis,       // Chẩn đoán bệnh
                        Month = g.Key.Month,               // Tháng trong năm
                        Year = g.Key.Year,                 // Năm
                        CaseCount = g.Count()              // Số lượng bản ghi trong nhóm (tức là số ca bệnh cho tháng đó)
                    })
                    .OrderBy(x => x.Year)                  // Sắp xếp theo năm
                    .ThenBy(x => x.Month)                  // Sắp xếp theo tháng
                    .ToList();                            // Chuyển đổi kết quả thành danh sách List<ComparisonRecord>
            }
        }


        private void cmbTimePeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            rtxtEpimedic.Clear();
            var records = GetMedicalRecords();
            var comparison = GetComparisonRecords(records, cmbTimePeriod.Text);
            NotifySuddenIncrease(comparison);
        }
    }
}
