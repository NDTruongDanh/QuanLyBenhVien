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
        private Dictionary<string, List<string>> GetPreviousWeekSchedule(DateTime startDate)
        {
            var previousWeekSchedule = new Dictionary<string, List<string>>();
            DateTime previousWeekStart = startDate.AddDays(-7);
            DateTime previousWeekEnd = previousWeekStart.AddDays(6);

            using (var connection = new SqlConnection(connStr))
            {
                connection.Open();
                string query = @"
        SELECT StaffID, ShiftType 
        FROM WEEKLYASSIGNMENT 
        WHERE WeekStartDate >= @PreviousWeekStart AND WeekEndDate <= @PreviousWeekEnd";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PreviousWeekStart", previousWeekStart);
                    command.Parameters.AddWithValue("@PreviousWeekEnd", previousWeekEnd);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string staffId = reader["StaffID"].ToString();
                            string shiftType = reader["ShiftType"].ToString();

                            if (!previousWeekSchedule.ContainsKey(staffId))
                            {
                                previousWeekSchedule[staffId] = new List<string>();
                            }
                            previousWeekSchedule[staffId].Add(shiftType);
                        }
                    }
                }
            }

            return previousWeekSchedule;
        }

        public void AssignShifts(DateTime startDate)
        {
            endDate = startDate.StartOfWeek(DayOfWeek.Monday).AddDays(6);

            if (IsScheduleExist(startDate, endDate))
            {
                return;
            }

            var staffList = GetStaffList();
            var shifts = new[] { "Sáng", "Chiều", "Tối" };
            int shiftIndex = 0;

            // Lấy lịch trực tuần trước
            var previousWeekSchedule = GetPreviousWeekSchedule(startDate);

            var schedule = new List<(string StaffID, DateTime Date, string ShiftType)>();

            foreach (var date in EachDay(startDate, endDate))
            {
                var dailySchedule = new Dictionary<string, int>(); // StaffID -> số ca trong ngày
                var shiftStaffCount = new Dictionary<string, int>(); // ShiftType -> số nhân viên trong ca

                // Khởi tạo số nhân viên cho từng ca
                foreach (var shift in shifts)
                {
                    shiftStaffCount[shift] = 0;
                }

                foreach (var shift in shifts)
                {
                    while (shiftStaffCount[shift] < 5)
                    {
                        for (int attempt = 0; attempt < staffList.Count; attempt++)
                        {
                            var staff = staffList[shiftIndex % staffList.Count];
                            shiftIndex++;

                            if (!dailySchedule.ContainsKey(staff.StaffID))
                            {
                                dailySchedule[staff.StaffID] = 0;
                            }

                            // Kiểm tra điều kiện phân ca
                            bool isValidShift =
                                (dailySchedule[staff.StaffID] < 2 || shiftStaffCount[shift] >= 5) // Điều kiện số ca trong ngày
                                && (!previousWeekSchedule.ContainsKey(staff.StaffID) ||
                                    !previousWeekSchedule[staff.StaffID].Contains(shift)); // Không trùng với tuần trước

                            if (isValidShift)
                            {
                                schedule.Add((staff.StaffID, date, shift));
                                dailySchedule[staff.StaffID]++;
                                shiftStaffCount[shift]++;
                                break;
                            }
                        }

                        // Nếu đủ 5 nhân viên trong ca, thoát vòng lặp
                        if (shiftStaffCount[shift] >= 5)
                            break;
                    }
                }

                // Đảm bảo nhân viên làm đủ 2 ca/ngày
                foreach (var staff in staffList)
                {
                    if (dailySchedule.TryGetValue(staff.StaffID, out int shiftsAssigned) && shiftsAssigned < 2)
                    {
                        foreach (var shift in shifts)
                        {
                            if (shiftStaffCount[shift] < 5)
                            {
                                schedule.Add((staff.StaffID, date, shift));
                                dailySchedule[staff.StaffID]++;
                                shiftStaffCount[shift]++;
                                if (dailySchedule[staff.StaffID] >= 2)
                                    break;
                            }
                        }
                    }
                }
            }

            SaveScheduleToDatabase(schedule);
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