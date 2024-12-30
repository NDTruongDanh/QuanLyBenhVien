using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien.Classes
{
    internal class CommonControls
    {
        static readonly string connStr = "Data Source=ADMIN-PC;Initial Catalog=HospitalDB;Integrated Security=True;";

        static ContextMenuStrip contextMenu;

        static TabControl tabControl;
        public static void ResetInputFields(Control parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            foreach (Control control in parent.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = -1;
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).Value = DateTime.Now;
                }
                else if (control.HasChildren)
                {
                    // Recursively check child controls
                    ResetInputFields(control);
                }
            }
        }


        public static void InitializeTabControl(TabControl tabCtrl)
        {
            tabControl = tabCtrl;
            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem closeTabMenuItem = new ToolStripMenuItem("Close Tab");
            closeTabMenuItem.Click += CloseTabMenuItem_Click;
            contextMenu.Items.Add(closeTabMenuItem);
            tabControl.MouseUp += TabControl_MouseUp;
        }

        public static void AddFormToTab(Form childForm, string tabName)
        {
            foreach (TabPage existingTab in tabControl.TabPages)
            {
                if (existingTab.Text == tabName)
                {
                    tabControl.SelectedTab = existingTab;
                    return;
                }
            }
            TabPage tabPage = new TabPage(tabName);
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            tabPage.Controls.Add(childForm);
            tabControl.TabPages.Add(tabPage);
            childForm.Size = tabControl.Size;
            childForm.Show();
            tabControl.SelectedTab = tabPage;
        }

        public static void TabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Identify the tab under the mouse pointer
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tabControl.SelectedIndex = i; // Select the tab
                        contextMenu.Show(tabControl, e.Location); // Show the context menu
                        break;
                    }
                }
            }
        }

        public static void CloseTabMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab); // Close the selected tab
            }
        }

        public static void InitializeCmbPatientID(ComboBox cmbPatientID)
        {
            string query = "SELECT PatientID FROM PATIENT";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbPatientID.Items.Add(reader["PatientID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbDoctorID(ComboBox cmbDoctorID)
        {
            string query = "SELECT StaffID FROM STAFF WHERE TypeOfStaff LIKE N'%Bác sĩ%'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbDoctorID.Items.Add(reader["StaffID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbDepartmentID(ComboBox cmbDepartmentID)
        {
            string query = "SELECT DepartmentID FROM DEPARTMENT";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbDepartmentID.Items.Add(reader["DepartmentID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializCmbMedicationID(ComboBox cmbMedicationID)
        {
            string query = "SELECT MedicationID FROM MEDICATION";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbMedicationID.Items.Add(reader["MedicationID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbRecordID(ComboBox cmbRecordID)
        {
            string query = "SELECT RecordID FROM MEDICALRECORD";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbRecordID.Items.Add(reader["RecordID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbStaffID(ComboBox cmbStaffID)
        {
            string query = "SELECT StaffID FROM STAFF";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbStaffID.Items.Add(reader["StaffID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbHeadID(ComboBox cmbHeadID, string departmentID)
        {
            cmbHeadID.Items.Clear();
            string query = $"SELECT StaffID FROM STAFF WHERE DepartmentID = '{departmentID}'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbHeadID.Items.Add(reader["StaffID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        public static void InitializeCmbNurseID(ComboBox cmbNurseID)
        {
            string query = "SELECT StaffID FROM STAFF WHERE TypeOfStaff LIKE N'%Điều dưỡng%'";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbNurseID.Items.Add(reader["StaffID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbTypeOfRoom(ComboBox cmbTypeOfRoom)
        {
            string query = "SELECT DISTINCT RoomType FROM ROOM";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbTypeOfRoom.Items.Add(reader["RoomType"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbRoomID(ComboBox cmbRoomID)
        {
            string query = "SELECT RoomID FROM ROOM";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbRoomID.Items.Add(reader["RoomID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbDosageUnint(ComboBox cmbDosageUnit)
        {
            string query = "SELECT DISTINCT DosageUnit FROM MEDICATION";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbDosageUnit.Items.Add(reader["DosageUnit"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void InitializeCmbCategory(ComboBox cmbCategory)
        {
            string query = "SELECT DISTINCT Category FROM MEDICATION";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbCategory.Items.Add(reader["Category"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public static void SetupDateTimePickerCustom_Time(DateTimePicker dateTimePicker)
        {
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "hh:mm tt dd/MM/yyyy"; // Định dạng ngày và giờ
            dateTimePicker.ShowUpDown = true; // Ẩn lịch, chỉ chọn giờ
        }

        public static void SetupDateTimePickerCustom_Date(DateTimePicker dateTimePicker)
        {
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "dd/MM/yyyy"; // Định dạng ngày và giờ
            dateTimePicker.ShowUpDown = true; // Ẩn lịch, chỉ chọn giờ 
        }

        public static void DisableRememberMe(string userID)
        {
            string query = "UPDATE USERLOGIN SET Flag = 0 WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open(); // Mở kết nối

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        int rowsAffected = command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Tắt nhớ mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }
    }
}
