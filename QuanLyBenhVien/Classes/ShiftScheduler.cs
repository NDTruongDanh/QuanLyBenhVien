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
        private readonly string connectionString = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";
        private readonly string[] _shiftTypes = new[] { "Sáng", "Chiều", "Tối" };
        private readonly int _staffPerShift = 5;
        private const int BATCH_SIZE = 50;

        // Class để lưu thông tin assignment tạm thời
        private class AssignmentData
        {
            public string AssignmentID { get; set; }
            public string StaffID { get; set; }
            public DateTime AssignmentDate { get; set; }
            public string ShiftType { get; set; }
        }

        // Lấy danh sách tất cả nhân viên
        private List<string> GetAllStaffIds()
        {
            List<string> staffIds = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT StaffID FROM STAFF WITH (NOLOCK)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 180;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staffIds.Add(reader["StaffID"].ToString().Trim());
                        }
                    }
                }
            }
            return staffIds;
        }

        // Kiểm tra xem ngày đã có lịch chưa
        private bool IsScheduleExist(DateTime date)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 1 FROM WEEKLYASSIGNMENT WITH (NOLOCK) WHERE CONVERT(DATE, AssignmentDate) = @Date";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddWithValue("@Date", date.Date);
                    conn.Open();
                    return cmd.ExecuteScalar() != null;
                }
            }
        }

        // Lấy danh sách nhân viên đã làm việc ngày hôm trước
        private List<string> GetStaffWorkedPreviousDay(DateTime date)
        {
            List<string> staffIds = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT DISTINCT StaffID 
                           FROM WEEKLYASSIGNMENT WITH (NOLOCK)
                           WHERE CONVERT(DATE, AssignmentDate) = @PreviousDate";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddWithValue("@PreviousDate", date.AddDays(-1).Date);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staffIds.Add(reader["StaffID"].ToString().Trim());
                        }
                    }
                }
            }
            return staffIds;
        }

        // Tạo lịch cho một ngày
        private List<AssignmentData> GenerateDailySchedule(DateTime date, List<string> allStaffIds)
        {
            var assignments = new List<AssignmentData>();
            var availableStaff = new List<string>(allStaffIds);

            // Loại bỏ nhân viên đã làm việc ngày hôm trước
            var previousDayStaff = GetStaffWorkedPreviousDay(date);
            foreach (var staff in previousDayStaff)
            {
                availableStaff.Remove(staff);
            }

            // Nếu không đủ nhân viên, bổ sung thêm từ danh sách đã làm việc
            int requiredStaff = _staffPerShift * _shiftTypes.Length;
            if (availableStaff.Count < requiredStaff)
            {
                var additionalStaff = previousDayStaff
                    .OrderBy(x => Guid.NewGuid())
                    .Take(requiredStaff - availableStaff.Count);
                availableStaff.AddRange(additionalStaff);
            }

            // Xáo trộn danh sách nhân viên
            availableStaff = availableStaff.OrderBy(x => Guid.NewGuid()).ToList();

            // Phân ca cho từng shift
            var assignedStaffToday = new HashSet<string>();
            foreach (var shift in _shiftTypes)
            {
                var staffForShift = availableStaff
                    .Where(s => !assignedStaffToday.Contains(s) || assignedStaffToday.Count >= availableStaff.Count - _staffPerShift)
                    .Take(_staffPerShift)
                    .ToList();

                foreach (var staffId in staffForShift)
                {
                    assignments.Add(new AssignmentData
                    {
                        AssignmentID = string.Empty, // Sẽ được gán khi lưu
                        StaffID = staffId,
                        AssignmentDate = date,
                        ShiftType = shift
                    });
                    assignedStaffToday.Add(staffId);
                }

                // Cập nhật danh sách available
                availableStaff = availableStaff.Except(staffForShift).ToList();
            }

            return assignments;
        }

        // Lưu lịch vào database
        private void SaveAssignmentsBatch(List<AssignmentData> assignments)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Lấy ID cuối cùng để tạo ID mới
                        string getLastIdQuery = @"SELECT ISNULL(MAX(CAST(SUBSTRING(AssignmentID, 3, LEN(AssignmentID) - 2) AS INT)), 0) 
                                           FROM WEEKLYASSIGNMENT WITH (TABLOCKX)
                                           WHERE AssignmentID LIKE 'WA%'";

                        int startNumber;
                        using (SqlCommand cmd = new SqlCommand(getLastIdQuery, conn, transaction))
                        {
                            cmd.CommandTimeout = 180;
                            startNumber = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                        }

                        // Gán ID cho tất cả assignments
                        for (int i = 0; i < assignments.Count; i++)
                        {
                            assignments[i].AssignmentID = $"WA{(startNumber + i):D4}";
                        }

                        // Insert theo batch
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = transaction;
                            cmd.CommandTimeout = 180;

                            for (int i = 0; i < assignments.Count; i += BATCH_SIZE)
                            {
                                var batch = assignments.Skip(i).Take(BATCH_SIZE);
                                var valueStrings = new List<string>();
                                var parameters = new List<SqlParameter>();
                                int paramCount = 0;

                                foreach (var assignment in batch)
                                {
                                    valueStrings.Add($"(@id{paramCount}, @staff{paramCount}, @date{paramCount}, @shift{paramCount})");
                                    parameters.Add(new SqlParameter($"@id{paramCount}", assignment.AssignmentID));
                                    parameters.Add(new SqlParameter($"@staff{paramCount}", assignment.StaffID));
                                    parameters.Add(new SqlParameter($"@date{paramCount}", assignment.AssignmentDate));
                                    parameters.Add(new SqlParameter($"@shift{paramCount}", assignment.ShiftType));
                                    paramCount++;
                                }

                                string query = $@"INSERT INTO WEEKLYASSIGNMENT (AssignmentID, StaffID, AssignmentDate, ShiftType) 
                                           VALUES {string.Join(",", valueStrings)}";

                                cmd.CommandText = query;
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddRange(parameters.ToArray());
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Phương thức chính để tạo lịch
        public void GenerateSchedule(DateTime startDate)
        {
            var allStaffIds = GetAllStaffIds();
            if (!allStaffIds.Any())
            {
                throw new Exception("Không có nhân viên nào trong hệ thống!");
            }

            // Kiểm tra số lượng nhân viên tối thiểu
            int minRequiredStaff = _staffPerShift * _shiftTypes.Length / 2;
            if (allStaffIds.Count < minRequiredStaff)
            {
                throw new Exception($"Cần tối thiểu {minRequiredStaff} nhân viên để lập lịch! Hiện tại chỉ có {allStaffIds.Count} nhân viên.");
            }

            // Lấy ngày đầu tuần
            DateTime weekStart = startDate.Date.AddDays(-(int)startDate.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime weekEnd = weekStart.AddDays(6);

            var allAssignments = new List<AssignmentData>();

            // Tạo lịch cho từng ngày trong tuần
            for (DateTime date = weekStart; date <= weekEnd; date = date.AddDays(1))
            {
                if (!IsScheduleExist(date))
                {
                    var dailyAssignments = GenerateDailySchedule(date, allStaffIds);
                    allAssignments.AddRange(dailyAssignments);
                }
            }

            // Lưu tất cả assignments vào database
            if (allAssignments.Any())
            {
                SaveAssignmentsBatch(allAssignments);
            }
        }
    }
}