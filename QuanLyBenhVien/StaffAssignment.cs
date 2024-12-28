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
    public partial class StaffAssignment : Form
    {
        private readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        string staffID = null;
        string staffName = null;

        public StaffAssignment()
        {
            InitializeComponent();
            cmbSelection.Text = "Tuần này";
            LoadWeeklyAssignments();
        }
        public StaffAssignment(string userID)
        {
            InitializeComponent();
            staffID = userID;
            cmbSelection.Text = "Tuần này";
            LoadWeeklyAssignments();
        }

        private void LoadWeeklyAssignments()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Query to fetch assignments for the current week
                    string sql = null;
                    if (cmbSelection.SelectedIndex == 1)
                    {
                        sql = $@"SELECT st.StaffID, FullName, AssignmentDate, ShiftType 
                               FROM WEEKLYASSIGNMENT w JOIN STAFF st ON w.StaffID = st.StaffID
                               WHERE AssignmentDate > DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) 
                                      AND AssignmentDate <= DATEADD(DAY, 15 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
                               ORDER BY ShiftType, AssignmentDate";
                    }
                    else
                    {
                        sql = $@"SELECT st.StaffID, FullName, AssignmentDate, ShiftType 
                               FROM WEEKLYASSIGNMENT w JOIN STAFF st ON w.StaffID = st.StaffID
                               WHERE AssignmentDate > DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE)) 
                                      AND AssignmentDate <= DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
                               ORDER BY ShiftType, AssignmentDate";
                    }
                        
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Clear existing data
                        dgvAssignment.Rows.Clear();

                        // Create rows for each shift type
                        string[] shifts = { "Sáng", "Chiều", "Tối" };
                        foreach (string shift in shifts)
                        {
                            dgvAssignment.Rows.Add(shift);
                        }

                        while (reader.Read())
                        {
                            string staffIDs = reader["StaffID"].ToString();
                            string staffNames = reader["FullName"].ToString();
                            string shiftType = reader["ShiftType"].ToString();
                            DateTime assignmentDate = (DateTime)reader["AssignmentDate"];

                            // Map ShiftType to the appropriate row index
                            int rowIndex = Array.IndexOf(shifts, shiftType);
                            if (rowIndex == -1)
                            {
                                MessageBox.Show($"ShiftType '{shiftType}' is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }


                            // Map Day of the Week to column index

                            int columnIndex = ((int)assignmentDate.DayOfWeek == 0 ? 6 : (int)assignmentDate.DayOfWeek - 1) + 1;
                            if (columnIndex >= 1 && columnIndex <= 7) // Ensure within range
                            {
                                if (!string.IsNullOrEmpty(dgvAssignment.Rows[rowIndex].Cells[columnIndex].Value?.ToString()))
                                {
                                    dgvAssignment.Rows[rowIndex].Cells[columnIndex].Value += "\n";
                                }
                                dgvAssignment.Rows[rowIndex].Cells[columnIndex].Value += staffNames;

                            }
               
                            if (staffID == staffIDs)
                            {
                                HighlightCellsWithString(staffNames);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWeeklyAssignments();
        }

        private void HighlightCellsWithString(string searchString)
        {
            foreach (DataGridViewRow row in dgvAssignment.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().Contains(searchString))
                    {
                        cell.Style.BackColor = Color.Yellow;
                    }
                }
            }
        }


    }
}
