using QuanLyBenhVien.Classes;
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
            dtt.Show();
        }

        private void btnYearIncomeReport_Click(object sender, EventArgs e)
        {
            DoanhthuNAM dtn = new DoanhthuNAM();
            dtn.Show();
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
            ktt.Show();
        }

        private void btnExpired_Click(object sender, EventArgs e)
        {
            KiemtraHANTHUOC ktht = new KiemtraHANTHUOC();
            ktht.Show();
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            EpidemicSituation epidemic = new EpidemicSituation();
            epidemic.Show();
        }


        private void btnWeeklyAssignment_Click(object sender, EventArgs e)
        {
            WeeklyAssignmentForm weeklyAssignment = new WeeklyAssignmentForm();
            CommonControls.AddFormToTab(weeklyAssignment, weeklyAssignment.Text);
        }
        private void btnMonthDiseaseStat_Click(object sender, EventArgs e)
        {
            BenhTheoThang btt = new BenhTheoThang();
            btt.Show();
        }

        private void btnYearDiseaseStat_Click(object sender, EventArgs e)
        {
            BenhTheoNam btn = new BenhTheoNam();
            btn.Show();
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
    }
}
