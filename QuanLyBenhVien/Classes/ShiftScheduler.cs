using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBenhVien.Classes
{
    public class ShiftScheduler
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        private bool IsScheduleExist(DateTime startDate, DateTime endDate)
        {
            using (var connection = new SqlConnection(connStr))
            {
                connection.Open();
                string query = @"
            SELECT COUNT(*) 
            FROM WEEKLYASSIGNMENT 
            WHERE WeekStartDate >= @StartDate AND WeekEndDate <= @EndDate";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private DateTime endDate;
        public void AssignShifts(DateTime startDate)
        {
            endDate = startDate.StartOfWeek(DayOfWeek.Monday).AddDays(6);
            MessageBox.Show(endDate.ToString());

            // Kiểm tra nếu lịch trực đã tồn tại
            if (IsScheduleExist(startDate, endDate))
            {
                MessageBox.Show("Lịch trực đã tồn tại. Không cần chia thêm.");
                return;
            }

            var staffList = GetStaffList();
            var shifts = new[] { "Sáng", "Chiều", "Tối" };
            int shiftIndex = 0;

            var schedule = new List<(string StaffID, DateTime Date, string ShiftType)>();

            foreach (var date in EachDay(startDate, endDate))
            {
                var dailySchedule = new Dictionary<string, int>(); // StaffID -> số ca hôm đó

                foreach (var shift in shifts)
                {
                    // Tìm nhân viên phù hợp theo Round-Robin
                    for (int attempt = 0; attempt < staffList.Count; attempt++)
                    {
                        var staff = staffList[shiftIndex % staffList.Count];
                        shiftIndex++;

                        if (!dailySchedule.ContainsKey(staff.StaffID))
                        {
                            dailySchedule[staff.StaffID] = 0;
                        }

                        if (dailySchedule[staff.StaffID] < 2) // Không quá 2 ca/ngày
                        {
                            schedule.Add((staff.StaffID, date, shift));
                            dailySchedule[staff.StaffID]++;
                            break;
                        }
                    }
                }
            }

            SaveScheduleToDatabase(schedule);
            MessageBox.Show("Lịch trực đã được chia.");
        }
        

        private List<Staff> GetStaffList()
        {
            var staffList = new List<Staff>();

            using (var connection = new SqlConnection(connStr))
            {
                connection.Open();
                string query = "SELECT StaffID, FullName FROM STAFF";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        staffList.Add(new Staff
                        {
                            StaffID = reader["StaffID"].ToString(),
                            FullName = reader["FullName"].ToString()
                        });
                    }
                }
            }

            return staffList;
        }

        private void SaveScheduleToDatabase(List<(string StaffID, DateTime Date, string ShiftType)> schedule)
        {
            using (var connection = new SqlConnection(connStr))
            {
                connection.Open();

                foreach (var entry in schedule)
                {
                    // Lấy DepartmentID của nhân viên từ bảng STAFF
                    string departmentId = GetDepartmentID(connection, entry.StaffID);

                    // Lấy số AssignmentID tiếp theo từ bảng WEEKLYASSIGNMENT
                    string assignmentId = GenerateAssignmentID(connection);

                    string query = @"
                INSERT INTO WEEKLYASSIGNMENT (AssignmentID, StaffID, DepartmentID, WeekStartDate, WeekEndDate, ShiftType)
                VALUES (@AssignmentID, @StaffID, @DepartmentID, @WeekStartDate, @WeekEndDate, @ShiftType)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AssignmentID", assignmentId);
                        command.Parameters.AddWithValue("@StaffID", entry.StaffID);
                        command.Parameters.AddWithValue("@DepartmentID", departmentId); // Chèn DepartmentID
                        command.Parameters.AddWithValue("@WeekStartDate", entry.Date);
                        command.Parameters.AddWithValue("@WeekEndDate", endDate); // Tuần kết thúc sau 6 ngày
                        command.Parameters.AddWithValue("@ShiftType", entry.ShiftType);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GetDepartmentID(SqlConnection connection, string staffId)
        {
            // Truy vấn để lấy DepartmentID từ bảng STAFF
            string query = "SELECT DepartmentID FROM STAFF WHERE StaffID = @StaffID";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StaffID", staffId);
                return (string)command.ExecuteScalar();
            }
        }
        private string GenerateAssignmentID(SqlConnection connection)
        {
            // Lấy số ID Assignment hiện tại từ bảng WEEKLYASSIGNMENT
            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(AssignmentID, 3, 4) AS INT)), 0) + 1 FROM WEEKLYASSIGNMENT";
            using (var command = new SqlCommand(query, connection))
            {
                int nextId = (int)command.ExecuteScalar();
                return $"WA{nextId:D4}";  // Định dạng 'WA' và số 4 chữ số (ví dụ: 'WA0001')
            }
        }

        private IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        {
            for (var day = start.Date; day <= end.Date; day = day.AddDays(1))
                yield return day;
        }
    }

    public class Staff
    {
        public string StaffID { get; set; }
        public string FullName { get; set; }
    }
}
