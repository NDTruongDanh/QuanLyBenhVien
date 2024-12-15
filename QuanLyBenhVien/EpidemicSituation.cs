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

        public class MedicalRecord
        {
            public DateTime VisitDate { get; set; }
            public string Diagnosis { get; set; }
        }

        public class ComparisonRecord
        {
            public string Diagnosis { get; set; }
            public DateTime VisitDate { get; set; }
            public int Week { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public int CaseCount { get; set; }
        }

        public class ChangeRecord
        {
            public string Diagnosis { get; set; }
            public DateTime VisitDate { get; set; }

            public int Week { get; set; }
            public int Month { get; set; }
            public int Year { get; set; }
            public int CaseCount { get; set; }
            public double ChangeRate { get; set; }
        }

        public static int GetISOWeekOfYear(DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

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
        
            if (cmbTimePeriod.Text == "Theo tuần")
            {
                timePeriod = current.AddDays(-14);
                query = "SELECT Diagnosis, VisitDate FROM MEDICALRECORD WHERE Diagnosis IN (N'Cảm cúm','Covid-19',N'Viêm phổi') AND VisitDate >= @Date";
            }
            else
            {
                timePeriod = current.AddMonths(-2);
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
                                    VisitDate = Convert.ToDateTime(reader["VisitDate"]),
                                    Diagnosis = reader["Diagnosis"].ToString()
                                };
                                records.Add(record);
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


        private void NotifySuddenIncrease(List<ComparisonRecord> comparison, double threshold = 50)
        {
            var groupedByDiagnosis = comparison
                .GroupBy(c => c.Diagnosis)
                .SelectMany(g =>
                {
                    var diagnosisRecords = g.OrderBy(r => r.Year).ThenBy(r => r.Month).ToList();

                    return diagnosisRecords.Select((current, index) =>
                    {
                        if (index == 0)
                            return null; // No previous record for comparison

                        var previous = diagnosisRecords[index - 1];

                        // Calculate the percentage change
                        double changeRate = previous.CaseCount == 0 ? (current.CaseCount > 0 ? 100 : 0) : ((current.CaseCount - previous.CaseCount) / (double)previous.CaseCount) * 100;

                        //If the change rate exceeds the threshold, notify
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
                    }).Where(x => x != null).ToList();
                }).ToList();

            //If any sudden increase is detected, show the details in a message box
            if (groupedByDiagnosis.Any())
            {
                StringBuilder notificationMessage = new StringBuilder();
                foreach (var record in groupedByDiagnosis)
                {
                    notificationMessage.AppendLine($"Loại bệnh: {record.Diagnosis}\n" +
                                    $"Số ca mắc bệnh tăng từ: {record.previousCaseCount} ({record.PreviousPeriod}) đến {record.currentCaseCount} ({record.CurrentPeriod})\n" +
                                    $"Tỷ lệ gia tăng: {record.ChangeRate:F2}%\n\n");
                }

                MessageBox.Show(notificationMessage.ToString(), "Phát hiện tỷ lệ mắc bệnh gia tăng đột ngột");
            }
            else
            {
                MessageBox.Show("Không phát hiện sự gia tăng bất thường.", "Dịch bệnh đang được kiểm so");
            }

            foreach (var record in groupedByDiagnosis)
            {
                // Tạo chuỗi văn bản cho mỗi chẩn đoán
                string recordText = $"Loại bệnh: {record.Diagnosis}\n" +
                                    $"Số ca mắc bệnh tăng từ: {record.previousCaseCount} ({record.PreviousPeriod}) đến {record.currentCaseCount} ({record.CurrentPeriod})\n" +
                                    $"Tỷ lệ gia tăng: {record.ChangeRate:F2}%\n\n";

                // Thêm chuỗi văn bản vào RichTextBox
                rtxtEpimedic.AppendText(recordText);
            }
        }


        private List<ChangeRecord> CalculateChangeRates(List<ComparisonRecord> comparison)
        {
            return comparison.Select((current, index) =>
            {
                if (index == 0)
                    return new ChangeRecord
                    {
                        Diagnosis = current.Diagnosis,
                        VisitDate = current.VisitDate,
                        Week = current.Week,
                        Month = current.Month,
                        Year = current.Year,
                        CaseCount = current.CaseCount,
                        ChangeRate = 0
                    };

                var previous = comparison[index - 1];
                return new ChangeRecord
                {
                    Diagnosis = current.Diagnosis,
                    VisitDate = current.VisitDate,
                    Week = current.Week,
                    Month = current.Month,
                    Year = current.Year,
                    CaseCount = current.CaseCount,
                    ChangeRate = previous.CaseCount == 0 ? 0 : ((current.CaseCount - previous.CaseCount) / (double)previous.CaseCount) * 100
                };
            }).ToList();
        }

        private List<ComparisonRecord> GetComparisonRecords(List<MedicalRecord> records, string period)
        {
            if (period == "Theo tuần")
            {
                return records
                    .GroupBy(r => new { r.Diagnosis, Week = GetISOWeekOfYear(r.VisitDate), r.VisitDate.Year })
                    .Select(g => new ComparisonRecord
                    {
                        Diagnosis = g.Key.Diagnosis,
                        Week = g.Key.Week,
                        Year = g.Key.Year,
                        CaseCount = g.Count()
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Week)
                    .ToList();
            }
            else
            {
                return records
                    .GroupBy(r => new { r.Diagnosis, r.VisitDate.Month, r.VisitDate.Year })
                    .Select(g => new ComparisonRecord
                    {
                        Diagnosis = g.Key.Diagnosis,
                        Month = g.Key.Month,
                        Year = g.Key.Year,

                        CaseCount = g.Count()
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToList();
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
