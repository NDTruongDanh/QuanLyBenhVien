using QuanLyBenhVien.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class MainForm : Form
    {
        string userID;
        public bool LogoutTriggered { get; set; }
        
        public MainForm(string userID)
        {
            InitializeComponent();
            CommonControls.InitializeTabControl(tabControl);
            this.userID = userID;
            LogoutTriggered = false;
            this.WindowState = FormWindowState.Maximized;

        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            PatientForm patientForm = new PatientForm();
            CommonControls.AddFormToTab(patientForm, patientForm.Text);
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            CommonControls.AddFormToTab(employeeForm, employeeForm.Text);
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            RoomForm roomForm = new RoomForm();
            CommonControls.AddFormToTab(roomForm, roomForm.Text);
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            MedicineForm medicineForm = new MedicineForm();
            CommonControls.AddFormToTab(medicineForm, medicineForm.Text);
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            DepartmentForm departmentForm = new DepartmentForm();
            CommonControls.AddFormToTab(departmentForm, departmentForm.Text);
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            BillForm billForm = new BillForm();
            CommonControls.AddFormToTab(billForm, billForm.Text);
        }

        private void btnMedicalRecord_Click(object sender, EventArgs e)
        {
            MedicalRecord medicalRecord = new MedicalRecord();
            CommonControls.AddFormToTab(medicalRecord, medicalRecord.Text);
        }

        private void btnMonthIncomeReport_Click(object sender, EventArgs e)
        {
            DoanhthuTHANG dtt = new DoanhthuTHANG();
            CommonControls.AddFormToTab(dtt,dtt.Text);
        }

        private void btnYearIncomeReport_Click(object sender, EventArgs e)
        {
            DoanhthuNAM dtn = new DoanhthuNAM();
            CommonControls.AddFormToTab(dtn, dtn.Text);
        }

        private void btnAIChatBot_Click(object sender, EventArgs e)
        {
            ChatBotForm chatBotForm = new ChatBotForm();
            CommonControls.AddFormToTab(chatBotForm, chatBotForm.Text);
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment();
            CommonControls.AddFormToTab(appointment, appointment.Text);
        }

        private void btnQuantityInStock_Click(object sender, EventArgs e)
        {
            KiemtraThuocTonKho ktt = new KiemtraThuocTonKho();
            CommonControls.AddFormToTab(ktt, ktt.Text);
        }

        private void btnExpired_Click(object sender, EventArgs e)
        {
            KiemtraHANTHUOC ktht = new KiemtraHANTHUOC();
            CommonControls.AddFormToTab(ktht, ktht.Text);
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            EpidemicSituation epidemic = new EpidemicSituation();
            CommonControls.AddFormToTab(epidemic, epidemic.Text);
        }


        private void btnWeeklyAssignment_Click(object sender, EventArgs e)
        {
            WeeklyAssignmentForm weeklyAssignmentForm = new WeeklyAssignmentForm();
            CommonControls.AddFormToTab(weeklyAssignmentForm, weeklyAssignmentForm.Text);
        }
        private void btnMonthDiseaseStat_Click(object sender, EventArgs e)
        {
            BenhTheoThang btt = new BenhTheoThang();
            CommonControls.AddFormToTab(btt, btt.Text);
        }

        private void btnYearDiseaseStat_Click(object sender, EventArgs e)
        {
            BenhTheoNam btn = new BenhTheoNam();
            CommonControls.AddFormToTab(btn, btn.Text);
        }

        private void btnChangePW_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword(userID);
            changePassword.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LogoutTriggered = true;
            this.Close();
        }

        private void chămSócBệnhNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NurseCareForm nurseCare = new NurseCareForm();
            CommonControls.AddFormToTab(nurseCare, nurseCare.Text);
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            AccountForm accountForm = new AccountForm();
            CommonControls.AddFormToTab(accountForm, accountForm.Text);
        }

        private void btnDeleteRemember_Click(object sender, EventArgs e)
        {
            CommonControls.DisableRememberMe(userID); 
        }

        private void btnHospitalize_Click(object sender, EventArgs e)
        {
            NhapVien nhapvien = new NhapVien();
            CommonControls.AddFormToTab(nhapvien, nhapvien.Text);
        }


        private void btnWorkAssignment_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Lấy ngày hiện tại
                DateTime currentDate = DateTime.Now;

                // Tạo lịch trực
                var scheduler = new ShiftScheduler();
                scheduler.GenerateSchedule(currentDate);
                scheduler.GenerateSchedule(currentDate.AddDays(7));
                MessageBox.Show("Đã tạo lịch trực thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Có lỗi xảy ra: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void thờiKhoáBiểuTrựcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StaffAssignment staffAssignment = new StaffAssignment();
            CommonControls.AddFormToTab(staffAssignment, staffAssignment.Text);
        }
    }
}
