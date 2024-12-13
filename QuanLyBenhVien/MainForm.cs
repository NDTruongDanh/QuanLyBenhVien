using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBenhVien
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void AddFormToTab(Form childForm, string tabName)
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

        private void btnPatient_Click(object sender, EventArgs e)
        {
           PatientForm patientForm = new PatientForm();
           AddFormToTab(patientForm, patientForm.Text);
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm(); 
            AddFormToTab(employeeForm, employeeForm.Text);
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            RoomForm roomForm = new RoomForm();
            AddFormToTab(roomForm, roomForm.Text);
        }

        private void btnMedicine_Click(object sender, EventArgs e)
        {
            MedicineForm medicineForm = new MedicineForm();
            AddFormToTab(medicineForm, medicineForm.Text);
        }

        private void btnMedicineStat_Click(object sender, EventArgs e)
        {
            MedStatForm medStatForm = new MedStatForm();
            AddFormToTab(medStatForm, medStatForm.Text);
        }

        private void khoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DepartmentForm departmentForm = new DepartmentForm();   
            AddFormToTab(departmentForm, departmentForm.Text);
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            BillForm billForm = new BillForm();
            AddFormToTab(billForm, billForm.Text);
        }

        private void btnMedicalRecord_Click(object sender, EventArgs e)
        {
            MedicalRecord medicalRecord = new MedicalRecord();
            AddFormToTab(medicalRecord, medicalRecord.Text);
        }

        private void thángToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoanhthuTHANG dtt = new DoanhthuTHANG();
            dtt.Show();
        }

        private void nămToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoanhthuNAM dtn = new DoanhthuNAM();
            dtn.Show();
        }
    }
}
